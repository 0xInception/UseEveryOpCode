using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace UseEveryOpCode.OpCodes;

public class JmpOpCode : IOpCode
{
    public IList<CilInstruction> CallingInstructions => new List<CilInstruction>();
    public MethodDefinition? Generate(TypeDefinition typeDefinition)
    {
        var name = CilOpCodes.Jmp.ToString().Replace(".", "_");
        var dummyMethod = new MethodDefinition($"{name}Dummy", MethodAttributes.Public | MethodAttributes.Static,
            new MethodSignature(CallingConventionAttributes.Default, typeDefinition.Module!.CorLibTypeFactory.Void,
                Enumerable.Empty<TypeSignature>()));
        dummyMethod.CilMethodBody = new CilMethodBody(dummyMethod);
        dummyMethod.CilMethodBody.Instructions.Add(CilOpCodes.Ret);
        typeDefinition.Methods.Add(dummyMethod);
        var method = new MethodDefinition(name, MethodAttributes.Public | MethodAttributes.Static,
            new MethodSignature(CallingConventionAttributes.Default, typeDefinition.Module!.CorLibTypeFactory.Void,
                Enumerable.Empty<TypeSignature>()));
        method.CilMethodBody = new CilMethodBody(method);
        method.CilMethodBody.Instructions.Add(CilOpCodes.Jmp, dummyMethod);
        method.CilMethodBody.Instructions.Add(CilOpCodes.Ret);
        return method;
    }
}