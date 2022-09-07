using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace UseEveryOpCode.OpCodes;
using static AsmResolver.PE.DotNet.Cil.CilOpCodes;

public class CallVirtOpCode : IOpCode
{
    public IList<CilInstruction> CallingInstructions => new List<CilInstruction>();

    public MethodDefinition? Generate(TypeDefinition typeDefinition)
    {

        var name = CilOpCodes.Callvirt.ToString().Replace(".", "_");
        var subType = new TypeDefinition(typeDefinition.Namespace, $"{name}Dummy",
            TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.NestedPublic,
            typeDefinition.Module.CorLibTypeFactory.Object.ToTypeDefOrRef());
        var callVirtMethod = new MethodDefinition(".ctor",
            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RuntimeSpecialName,
            MethodSignature.CreateInstance(typeDefinition.Module.CorLibTypeFactory.Void));
        callVirtMethod.CilMethodBody = new CilMethodBody(callVirtMethod)
        {
            Instructions =
            {
                Ret
            }
        };
        subType.Methods.Add(callVirtMethod);
        var dummyMethod = new MethodDefinition($"{name}DummyMethod", MethodAttributes.Public,
            MethodSignature.CreateInstance(typeDefinition.Module.CorLibTypeFactory.Void));
        dummyMethod.CilMethodBody = new CilMethodBody(dummyMethod)
        {
            Instructions =
            {
                { Ret }
            }
        };
        subType.Methods.Add(dummyMethod);
        typeDefinition.NestedTypes.Add(subType);
        subType.ImportWith(new ReferenceImporter(typeDefinition.Module));
        var method = new MethodDefinition(CilOpCodes.Callvirt.ToString().Replace(".", "_"),
            MethodAttributes.Public | MethodAttributes.Static,
            new MethodSignature(CallingConventionAttributes.Default, typeDefinition.Module!.CorLibTypeFactory.Void,
                Enumerable.Empty<TypeSignature>()));
        method.CilMethodBody = new CilMethodBody(method)
        {
            Instructions =
            {
                { Newobj, callVirtMethod },
                { Callvirt, dummyMethod },
                { Ret }
            }
        };
        return method;
    }
}