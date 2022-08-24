
using AsmResolver.PE.DotNet.Cil;

namespace UseEveryOpCode.OpCodes;

public class OpCodeService
{
    private Dictionary<CilOpCode, IOpCode> _opCodes;
    public OpCodeService()
    {
        var arithmeticOpCodes = new[]
        {
            CilOpCodes.Add,
            CilOpCodes.Add_Ovf,
            CilOpCodes.Add_Ovf_Un,
            CilOpCodes.Div,
            CilOpCodes.Div_Un,
            CilOpCodes.Mul,
            CilOpCodes.Mul_Ovf,
            CilOpCodes.Mul_Ovf_Un,
            CilOpCodes.Rem,
            CilOpCodes.Rem_Un,
            CilOpCodes.Sub,
            CilOpCodes.Sub_Ovf,
            CilOpCodes.Sub_Ovf_Un,
            CilOpCodes.Xor,
            CilOpCodes.Cgt,
            CilOpCodes.Cgt_Un, 
            CilOpCodes.Clt, 
            CilOpCodes.Clt_Un, 
            CilOpCodes.Ceq, 
            CilOpCodes.Shr, 
            CilOpCodes.Shr_Un, 
            CilOpCodes.Shl, 
            CilOpCodes.And, 
        };
        _opCodes = new Dictionary<CilOpCode, IOpCode>();
        foreach (var opcode in arithmeticOpCodes)
        {
            _opCodes.Add(opcode, new TwoPopOperatorOpCode(opcode));
        }
    }
    public IOpCode GetOpCode(CilOpCode opCode)
    {
        return _opCodes[opCode];
    }

    public (CilOpCode,IOpCode)[] GetOpCodes()
    {
        return _opCodes.Select(x=>(x.Key,x.Value)).ToArray();
    }
}