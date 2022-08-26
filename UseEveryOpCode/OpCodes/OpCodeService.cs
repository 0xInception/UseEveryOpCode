
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
        _opCodes.Add(CilOpCodes.Ldc_I8, new LdcI8OpCode());
        _opCodes.Add(CilOpCodes.Ldc_R4, new LdcR4OpCode());
        _opCodes.Add(CilOpCodes.Ldc_R8, new LdcR8OpCode());
        _opCodes.Add(CilOpCodes.Ldstr, new LdstrOpCode());

        var noPushNoPopOpCodes = new[]
        {
            CilOpCodes.Nop,
            CilOpCodes.Break,
            CilOpCodes.Ret,
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

        var convertOpCodes = new[]
        {
            CilOpCodes.Conv_I1,
            CilOpCodes.Conv_I2,
            CilOpCodes.Conv_I4,
            CilOpCodes.Conv_I8,
            CilOpCodes.Conv_R4,
            CilOpCodes.Conv_R8,
            CilOpCodes.Conv_U1,
            CilOpCodes.Conv_U2,
            CilOpCodes.Conv_U4,
            CilOpCodes.Conv_U8,
            CilOpCodes.Conv_I,
            CilOpCodes.Conv_U,
            CilOpCodes.Conv_R_Un,
            CilOpCodes.Conv_Ovf_I1,
            CilOpCodes.Conv_Ovf_I1_Un,
            CilOpCodes.Conv_Ovf_I2,
            CilOpCodes.Conv_Ovf_I2_Un,
            CilOpCodes.Conv_Ovf_I4,
            CilOpCodes.Conv_Ovf_I4_Un,
            CilOpCodes.Conv_Ovf_I8,
            CilOpCodes.Conv_Ovf_I8_Un,
            CilOpCodes.Conv_Ovf_U1,
            CilOpCodes.Conv_Ovf_U1_Un,
            CilOpCodes.Conv_Ovf_U2,
            CilOpCodes.Conv_Ovf_U2_Un,
            CilOpCodes.Conv_Ovf_U4,
            CilOpCodes.Conv_Ovf_U4_Un,
            CilOpCodes.Conv_Ovf_U8,
            CilOpCodes.Conv_Ovf_U8_Un,
            CilOpCodes.Conv_Ovf_I,
            CilOpCodes.Conv_Ovf_I_Un,
            CilOpCodes.Conv_Ovf_U,
            CilOpCodes.Conv_Ovf_U_Un, 
        };
        
        foreach (var opcode in convertOpCodes)
        {
            _opCodes.Add(opcode, new ConvertOpCode(opcode));
        }

        _opCodes.Add(CilOpCodes.Jmp, new JmpOpCode());

        var loadArgumentNoOperand = new[]
        {
            (CilOpCodes.Ldarg_0,1),
            (CilOpCodes.Ldarg_1,2),
            (CilOpCodes.Ldarg_2,3),
            (CilOpCodes.Ldarg_3,4)
        };

        foreach (var opcode in loadArgumentNoOperand)
        {
            _opCodes.Add(opcode.Item1, new LoadArgumentNoOperandOpCode(opcode.Item1,opcode.Item2));
        }

        var loadArgumentOpCodes = new[]
        {
            CilOpCodes.Ldarg, 
            CilOpCodes.Ldarga,
            CilOpCodes.Ldarg_S, 
            CilOpCodes.Ldarga_S, 
        };
        
        foreach (var opcode in loadArgumentOpCodes)
        {
            _opCodes.Add(opcode, new LoadArgumentOpCode(opcode));
        }
        
        var setArgumentOpCodes = new[]
        {
            CilOpCodes.Starg,
            CilOpCodes.Starg_S,
        };
        
        foreach (var opcode in setArgumentOpCodes)
        {
            _opCodes.Add(opcode, new SetArgumentOpCode(opcode));
        }

        var loadStaticFieldOpCodes = new[]
        {
            CilOpCodes.Ldsfld,
            CilOpCodes.Ldsflda,
        };
        
        foreach (var opcode in loadStaticFieldOpCodes)
        {
            _opCodes.Add(opcode, new LoadStaticFieldOpCode(opcode));
        }


        var loadFieldOpCodes = new[]
        {
            CilOpCodes.Ldfld,
            CilOpCodes.Ldflda
        };
        
        foreach (var opcode in loadFieldOpCodes)
        {
            _opCodes.Add(opcode, new LoadFieldOpCode(opcode));
        }
        

        _opCodes.Add(CilOpCodes.Stfld,new SetFieldOpCode(CilOpCodes.Stfld));
        _opCodes.Add(CilOpCodes.Stsfld,new SetStaticFieldOpCode(CilOpCodes.Stsfld));

        var loadLocalNoOperandOpCodes = new[]
        {
            (CilOpCodes.Ldloc_0, 1),
            (CilOpCodes.Ldloc_1, 2),
            (CilOpCodes.Ldloc_2, 3),
            (CilOpCodes.Ldloc_3, 4)
        };
        
        foreach (var opcode in loadLocalNoOperandOpCodes)
        {
            _opCodes.Add(opcode.Item1, new LoadLocalNoOperandOpCode(opcode.Item1, opcode.Item2));
        }
        
        var setLocalNoOperandOpCodes = new[]
        {
            (CilOpCodes.Stloc_0, 1),
            (CilOpCodes.Stloc_1, 2),
            (CilOpCodes.Stloc_2, 3),
            (CilOpCodes.Stloc_3, 4)
        };
        
        foreach (var opcode in setLocalNoOperandOpCodes)
        {
            _opCodes.Add(opcode.Item1, new SetLocalNoOperandOpCode(opcode.Item1, opcode.Item2));
        }

        var loadLocalOpCodes = new[]
        {
            CilOpCodes.Ldloc,
            CilOpCodes.Ldloc_S,
            CilOpCodes.Ldloca,
            CilOpCodes.Ldloca_S,
        };
        
        foreach (var opcode in loadLocalOpCodes)
        {
            _opCodes.Add(opcode, new LoadLocalOpCode(opcode));
        }

        var setLocalOpCodes = new[]
        {
            CilOpCodes.Stloc,
            CilOpCodes.Stloc_S,
        };
        
        foreach (var opcode in setLocalOpCodes)
        {
            _opCodes.Add(opcode, new SetLocalOpCode(opcode));
        }
        
        _opCodes.Add(CilOpCodes.Dup,new DupOpCode());
        _opCodes.Add(CilOpCodes.Pop,new PopOpCode());

    }

    public (CilOpCode,IOpCode)[] GetOpCodes()
    {
        return _opCodes.Select(x=>(x.Key,x.Value)).ToArray();
    }
}