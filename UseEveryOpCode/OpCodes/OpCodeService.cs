
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

        var singlePushOpCodes = new[]
        {
            CilOpCodes.Ldc_I4_0,
            CilOpCodes.Ldc_I4_1,
            CilOpCodes.Ldc_I4_2,
            CilOpCodes.Ldc_I4_3,
            CilOpCodes.Ldc_I4_4,
            CilOpCodes.Ldc_I4_5,
            CilOpCodes.Ldc_I4_6,
            CilOpCodes.Ldc_I4_7,
            CilOpCodes.Ldc_I4_8,
            CilOpCodes.Ldnull,
            CilOpCodes.Ldc_I4_M1,
        };

        foreach (var opcode in singlePushOpCodes)
        {
            _opCodes.Add(opcode, new SinglePushOpCode(opcode));
        }

        var ldcInstructions = new[]
        {
            CilOpCodes.Ldc_I4,
            CilOpCodes.Ldc_I4_S,
        };
        
        foreach (var opcode in ldcInstructions)
        {
            _opCodes.Add(opcode, new LdcI4OpCode(opcode));
        }

        _opCodes.Add(CilOpCodes.Ldc_R4, new LdcR4OpCode());
        _opCodes.Add(CilOpCodes.Ldc_R8, new LdcR8OpCode());
        _opCodes.Add(CilOpCodes.Ldstr, new LdstrOpCode());

        var noPushNoPopOpCodes = new[]
        {
            CilOpCodes.Nop,
            CilOpCodes.Break,
        };

        foreach (var opcode in noPushNoPopOpCodes)
        {
            _opCodes.Add(opcode, new NoPushNoPopOpCode(opcode));
        }

        _opCodes.Add(CilOpCodes.Call, new CallOpCode());
        _opCodes.Add(CilOpCodes.Calli, new CalliOpCode());
        _opCodes.Add(CilOpCodes.Callvirt, new CallVirtOpCode());

        var twoPopBranching = new[]
        {
            CilOpCodes.Beq,
            CilOpCodes.Beq_S,
            CilOpCodes.Bge,
            CilOpCodes.Bge_S,
            CilOpCodes.Bge_Un,
            CilOpCodes.Bge_Un_S,
            CilOpCodes.Bgt,
            CilOpCodes.Bgt_S,
            CilOpCodes.Bgt_Un,
            CilOpCodes.Bgt_Un_S,
            CilOpCodes.Ble,
            CilOpCodes.Ble_S,
            CilOpCodes.Ble_Un,
            CilOpCodes.Ble_Un_S,
            CilOpCodes.Blt,
            CilOpCodes.Blt_S,
            CilOpCodes.Blt_Un,
            CilOpCodes.Blt_Un_S,
            CilOpCodes.Bne_Un,
            CilOpCodes.Bne_Un_S,
        };

        foreach (var opcode in twoPopBranching)
        {
            _opCodes.Add(opcode, new TwoPopBranching(opcode));
        }

        var onePopBranching = new[]
        {
            CilOpCodes.Brfalse,
            CilOpCodes.Brfalse_S,
            CilOpCodes.Brtrue,
            CilOpCodes.Brtrue_S,
        };
        foreach (var opcode in onePopBranching)
        {
            _opCodes.Add(opcode, new OnePopBranching(opcode));
        }
        
        var unconditionalBranching = new[]
        {
            CilOpCodes.Br,
            CilOpCodes.Br_S,
        };
        foreach (var opcode in unconditionalBranching)
        {
            _opCodes.Add(opcode, new UnconditionalBranching(opcode));
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