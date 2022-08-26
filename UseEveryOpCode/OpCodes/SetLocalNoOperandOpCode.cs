using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace UseEveryOpCode.OpCodes;

public class SetLocalNoOperandOpCode : IOpCode
{
    private readonly CilOpCode _opCode;
    private readonly int _local;

    public SetLocalNoOperandOpCode(CilOpCode opCode,int local)
    {
        _opCode = opCode;
        _local = local;
    }


    public IList<CilInstruction> CallingInstructions => new List<CilInstruction>();
    public MethodDefinition? Generate(TypeDefinition typeDefinition)
    {
        var method = new MethodDefinition(_opCode.ToString().Replace(".","_"), MethodAttributes.Public | MethodAttributes.Static,
            new MethodSignature(CallingConventionAttributes.Default, typeDefinition.Module!.CorLibTypeFactory.Void,
                Enumerable.Empty<TypeSignature>()));
        method.CilMethodBody = new CilMethodBody(method);
        for(int i = 0;i<_local;i++)
        {
            method.CilMethodBody.LocalVariables.Add(new CilLocalVariable(typeDefinition.Module.CorLibTypeFactory.Int32));
        }
        method.CilMethodBody.Instructions.Add(new CilInstruction(CilOpCodes.Ldc_I4_0));
        method.CilMethodBody.Instructions.Add(_opCode);
        method.CilMethodBody.Instructions.Add(CilOpCodes.Ret);
        return method;
    }
}