using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace UseEveryOpCode.OpCodes;

public class SetArgumentOpCode : IOpCode
{
    private readonly CilOpCode _opCode;

    public SetArgumentOpCode(CilOpCode opCode)
    {
        _opCode = opCode;
    }

    public IList<CilInstruction> CallingInstructions => new List<CilInstruction>()
    {
        new (CilOpCodes.Ldc_I4_0)
    };

    public MethodDefinition? Generate(TypeDefinition typeDefinition)
    {
        var method = new MethodDefinition(_opCode.ToString().Replace(".","_"), MethodAttributes.Public | MethodAttributes.Static,
            new MethodSignature(CallingConventionAttributes.Default, typeDefinition.Module!.CorLibTypeFactory.Void, new []{typeDefinition.Module.CorLibTypeFactory.Int32}));
        method.CilMethodBody = new CilMethodBody(method);
        method.CilMethodBody.Instructions.Add(CilOpCodes.Ldc_I4_0);
        method.CilMethodBody.Instructions.Add(_opCode,method.Parameters.First());
        method.CilMethodBody.Instructions.Add(CilOpCodes.Ret);
        return method;
    }
}