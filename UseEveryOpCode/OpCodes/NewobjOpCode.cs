using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace UseEveryOpCode.OpCodes;
using static CilOpCodes;

public class NewobjOpCode : IOpCode
{
    public IList<CilInstruction> CallingInstructions => new List<CilInstruction>();

    public MethodDefinition? Generate(TypeDefinition typeDefinition)
    {

        var name = CilOpCodes.Newobj.ToString();
        var subType = new TypeDefinition(typeDefinition.Namespace, $"{name}Dummy",
            TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.NestedPublic,
            typeDefinition.Module.CorLibTypeFactory.Object.ToTypeDefOrRef());
        var newObj = new MethodDefinition(".ctor",
            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RuntimeSpecialName,
            MethodSignature.CreateInstance(typeDefinition.Module.CorLibTypeFactory.Void));
        newObj.CilMethodBody = new CilMethodBody(newObj)
        {
            Instructions = { { Ret } }
        };
        subType.Methods.Add(newObj);
        var dummyMethod = new MethodDefinition($"{name}DummyMethod", MethodAttributes.Public,
            MethodSignature.CreateInstance(typeDefinition.Module.CorLibTypeFactory.Void));
        dummyMethod.CilMethodBody = new CilMethodBody(dummyMethod)
        {
            Instructions = { { Ret } }
        };
        subType.Methods.Add(dummyMethod);
        typeDefinition.NestedTypes.Add(subType);
        subType.ImportWith(new ReferenceImporter(typeDefinition.Module));
        var method = new MethodDefinition(name,
            MethodAttributes.Public | MethodAttributes.Static,
            new MethodSignature(CallingConventionAttributes.Default, typeDefinition.Module!.CorLibTypeFactory.Void,
                Enumerable.Empty<TypeSignature>()));
        method.CilMethodBody = new CilMethodBody(method)
        {
            Instructions =
            {
                { Newobj, newObj },
                { Pop },
                { Ret }
            }
        };
        return method;
    }
}