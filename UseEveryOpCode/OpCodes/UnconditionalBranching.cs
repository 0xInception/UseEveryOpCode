using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace UseEveryOpCode.OpCodes;
using static CilOpCodes;

public class UnconditionalBranching : IOpCode
{
    private readonly CilOpCode _opCode;

    public UnconditionalBranching(CilOpCode opCode)
    {
        _opCode = opCode;
    }

    public IList<CilInstruction> CallingInstructions => new List<CilInstruction>();

    public MethodDefinition? Generate(TypeDefinition typeDefinition)
    {
        var method = new MethodDefinition(_opCode.ToString().Replace(".", "_"),
            MethodAttributes.Public | MethodAttributes.Static,
            new MethodSignature(CallingConventionAttributes.Default, typeDefinition.Module!.CorLibTypeFactory.Void,
                Enumerable.Empty<TypeSignature>()));
        var ret = new CilInstruction(CilOpCodes.Ret);
        method.CilMethodBody = new CilMethodBody(method)
        {
            Instructions =
            {
                { _opCode, ret.CreateLabel() },
                { ret }
            }
        };
        method.CilMethodBody.Instructions.CalculateOffsets();
        return method;
    }
}