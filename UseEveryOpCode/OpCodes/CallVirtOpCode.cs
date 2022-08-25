using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace UseEveryOpCode.OpCodes;

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
        callVirtMethod.CilMethodBody = new CilMethodBody(callVirtMethod);
        callVirtMethod.CilMethodBody.Instructions.Add(CilOpCodes.Ret);
        subType.Methods.Add(callVirtMethod);
        var dummyMethod = new MethodDefinition($"{name}DummyMethod", MethodAttributes.Public,
            MethodSignature.CreateInstance(typeDefinition.Module.CorLibTypeFactory.Void));
        dummyMethod.CilMethodBody = new CilMethodBody(dummyMethod);
        dummyMethod.CilMethodBody.Instructions.Add(CilOpCodes.Ret);
        subType.Methods.Add(dummyMethod);
        typeDefinition.NestedTypes.Add(subType);
        subType.ImportWith(new ReferenceImporter(typeDefinition.Module));
        var method = new MethodDefinition(CilOpCodes.Callvirt.ToString().Replace(".", "_"),
            MethodAttributes.Public | MethodAttributes.Static,
            new MethodSignature(CallingConventionAttributes.Default, typeDefinition.Module!.CorLibTypeFactory.Void,
                Enumerable.Empty<TypeSignature>()));
        method.CilMethodBody = new CilMethodBody(method);
        method.CilMethodBody.Instructions.Add(CilOpCodes.Newobj, callVirtMethod);
        method.CilMethodBody.Instructions.Add(CilOpCodes.Callvirt, dummyMethod);
        method.CilMethodBody.Instructions.Add(CilOpCodes.Ret);
        return method;
    }
}