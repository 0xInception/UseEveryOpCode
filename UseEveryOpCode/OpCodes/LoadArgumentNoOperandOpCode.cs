using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace UseEveryOpCode.OpCodes;
using static CilOpCodes;

public class LoadArgumentNoOperandOpCode : IOpCode
{
    private readonly CilOpCode _opCode;
    private readonly int _arguments;

    public LoadArgumentNoOperandOpCode(CilOpCode opCode, int arguments)
    {
        _opCode = opCode;
        _arguments = arguments;
    }

    public IList<CilInstruction> CallingInstructions
    {
        get
        {
            var instructions = new List<CilInstruction>();
            for (int i = 0; i < _arguments; i++)
            {
                instructions.Add(new CilInstruction(CilOpCodes.Ldc_I4, i));
            }

            return instructions;
        }
    }

    public MethodDefinition? Generate(TypeDefinition typeDefinition)
    {
        var method = new MethodDefinition(_opCode.ToString().Replace(".", "_"),
            MethodAttributes.Public | MethodAttributes.Static,
            new MethodSignature(CallingConventionAttributes.Default, typeDefinition.Module!.CorLibTypeFactory.Void,
                Enumerable.Repeat(typeDefinition.Module.CorLibTypeFactory.Int32, _arguments)));
        method.CilMethodBody = new CilMethodBody(method)
        {
            Instructions =
            {
                { _opCode },
                { Pop },
                { Ret }
            }
        };
        return method;
    }
}