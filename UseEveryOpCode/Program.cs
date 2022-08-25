// See https://aka.ms/new-console-template for more information

using AsmResolver;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;
using Sharprompt;
using UseEveryOpCode;
using UseEveryOpCode.OpCodes;
using UseEveryOpCode.Setup;

Console.ForegroundColor = ConsoleColor.White;
Console.Title = "UseEveryOpCode";
var service = new OpCodeService();
var opcodes = service.GetOpCodes();
var name = Prompt.Input<string>("Application name", "OpCodeTestApp",
    validators: new[] { Validators.Required(), Validators.MinLength(1), Validators.MaxLength(100) });
//var addAttributes = Prompt.Select("Add attributes indicating the opcode name", new[] { "Yes", "No" }, 2,"Yes") == "Yes"; TODO: Implement marking methods using custom attributes.
var runtime = Prompt.Select<Runtime>("Runtime");
var output = Prompt.Input<string>("Output Folder", Environment.CurrentDirectory,
    validators: new[] { Validators.Required(), Validators.MinLength(1), Validators.MaxLength(100) });
var excludedOpcodes = Prompt.MultiSelect("Exclude opcodes", opcodes.Select(d => d.Item1), 20, 0)
    .Select(x => opcodes.First(d => d.Item1 == x)).ToArray();
var includedAttributes = Prompt.List(new ListOptions<AttributeOptions>
{
    Message = "Included attributes ([FeatureName]|[Exclude]|[ApplyToMembers])",
    DefaultValues = new List<AttributeOptions>(),
    Minimum = 0
}).ToArray();

var resourceHelper = new ResourceHelper(runtime);

var module = ModuleDefinition.FromBytes(resourceHelper.ExtractModuleBytes());
var importer = new ReferenceImporter(module);
var type = module.CorLibTypeFactory.CorLibScope.CreateTypeReference("System.Reflection", "ObfuscationAttribute")
    .ImportWith(importer);
var constructor = type.CreateMemberReference(".ctor",
    new MethodSignature(CallingConventionAttributes.Default, type.ToTypeSignature(),
        Enumerable.Empty<TypeSignature>())).ImportWith(importer);



var assembly = module.CorLibTypeFactory.CorLibScope;
if (runtime != Runtime.DotNetFramework)
{
    assembly = module.AssemblyReferences.First(d => d.Name == "System.Console");
}

var writeLine = assembly.CreateTypeReference("System", "Console").CreateMemberReference("WriteLine",
    MethodSignature.CreateStatic(
        module.CorLibTypeFactory.Void, module.CorLibTypeFactory.String)).ImportWith(importer);

var programType = module.TopLevelTypes.First(d => d.Name == "Program");
var main = programType.Methods.First(d => d.Name == "Main");
main.CilMethodBody = new CilMethodBody(main);
foreach (var opCode in opcodes)
{
    if (excludedOpcodes.Contains(opCode))
        continue;
    var result = opCode.Item2.Generate(programType);
    if (result is null)
        continue;

    foreach (var attr in includedAttributes)
    {
        var attributeSignature = new CustomAttributeSignature
        {
            NamedArguments =
            {
                new CustomAttributeNamedArgument(CustomAttributeArgumentMemberType.Property, new Utf8String("Feature"),
                    module.CorLibTypeFactory.String,
                    new CustomAttributeArgument(module.CorLibTypeFactory.String, attr.Feature))
            }
        };
        if (attr.Exclude is not null)
            attributeSignature.NamedArguments.Add(new CustomAttributeNamedArgument(
                CustomAttributeArgumentMemberType.Property, new Utf8String("Exclude"), module.CorLibTypeFactory.Boolean,
                new CustomAttributeArgument(module.CorLibTypeFactory.Boolean, attr.Exclude)));
        if (attr.ApplyToMembers is not null)
            attributeSignature.NamedArguments.Add(new CustomAttributeNamedArgument(
                CustomAttributeArgumentMemberType.Property, new Utf8String("ApplyToMembers"),
                module.CorLibTypeFactory.Boolean,
                new CustomAttributeArgument(module.CorLibTypeFactory.Boolean, attr.ApplyToMembers)));
        
        result.CustomAttributes.Add(new CustomAttribute(constructor, attributeSignature));
    }

    programType.Methods.Add(result);

    main.CilMethodBody.Instructions.Add(new CilInstruction(CilOpCodes.Ldstr, $"Calling method {result}"));
    main.CilMethodBody.Instructions.Add(new CilInstruction(CilOpCodes.Call, writeLine));
    main.CilMethodBody.Instructions.AddRange(opCode.Item2.CallingInstructions);
    main.CilMethodBody.Instructions.Add(new CilInstruction(CilOpCodes.Call, result));
}

main.CilMethodBody.Instructions.Add(CilOpCodes.Ret);
var outName = module.Name!.Value.Replace("OpCodeTestApp", name.Replace(" ", "_"));
var path = Path.Combine(output, outName);
module.Write(path);
resourceHelper.WriteRuntimes(output, name.Replace(" ", "_"));
Console.WriteLine($"Wrote file and runtimes to {path}");
Console.WriteLine("Press any key to continue...");
Console.ReadKey(true);