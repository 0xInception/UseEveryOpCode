using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace UseEveryOpCode.OpCodes;
using static CilOpCodes;

public class LdindOpCode : IOpCode
{
    private readonly CilOpCode _opCode;

    public LdindOpCode(CilOpCode opCode)
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
        var v = new CilLocalVariable(typeDefinition.Module.CorLibTypeFactory.Int32);
        method.CilMethodBody = new CilMethodBody(method)
        {
            LocalVariables = { v },
            Instructions =
            {
                { Ldc_I4_1 },
                { Stloc_0 },
                { Ldloca, v },
                { _opCode },
                { Pop },
                { Ret }
            }
        };
        return method;
    }
}