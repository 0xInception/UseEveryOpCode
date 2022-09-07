using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace UseEveryOpCode.OpCodes;
using static CilOpCodes;

public class LdelemOpCode : IOpCode
{

    public IList<CilInstruction> CallingInstructions => new List<CilInstruction>();

    public MethodDefinition? Generate(TypeDefinition typeDefinition)
    {
        var method = new MethodDefinition(CilOpCodes.Ldelem.ToString(),
            MethodAttributes.Public | MethodAttributes.Static,
            new MethodSignature(CallingConventionAttributes.Default, typeDefinition.Module!.CorLibTypeFactory.Void,
                Enumerable.Empty<TypeSignature>()));
        var i32 = typeDefinition.Module.CorLibTypeFactory.Int32.ToTypeDefOrRef();
        method.CilMethodBody = new CilMethodBody(method)
        {
            Instructions =
            {
                { Ldc_I4_1 },
                { Newarr, i32 },
                { Ldc_I4_0 },
                { Ldelem, i32 },
                { Pop },
                { Ret }
            }
        };
        return method;
    }
}

public class LdelemaOpCode : IOpCode
{

    public IList<CilInstruction> CallingInstructions => new List<CilInstruction>();

    public MethodDefinition? Generate(TypeDefinition typeDefinition)
    {
        var method = new MethodDefinition(CilOpCodes.Ldelema.ToString(),
            MethodAttributes.Public | MethodAttributes.Static,
            new MethodSignature(CallingConventionAttributes.Default, typeDefinition.Module!.CorLibTypeFactory.Void,
                Enumerable.Empty<TypeSignature>()));
        var i32 = typeDefinition.Module.CorLibTypeFactory.Int32.ToTypeDefOrRef();
        method.CilMethodBody = new CilMethodBody(method)
        {
            Instructions =
            {
                { Ldc_I4_1 },
                { Newarr, i32 },
                { Ldc_I4_0 },
                { Ldelema, i32 },
                { Pop },
                { Ret }
            }
        };
        return method;
    }
}