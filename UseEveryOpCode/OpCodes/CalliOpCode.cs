using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace UseEveryOpCode.OpCodes;

using static AsmResolver.PE.DotNet.Cil.CilOpCodes;

public class CalliOpCode : IOpCode
{
    public IList<CilInstruction> CallingInstructions => new List<CilInstruction>();

    public MethodDefinition? Generate(TypeDefinition typeDefinition)
    {
        var name = CilOpCodes.Calli.ToString().Replace(".", "_");
        var dummyMethod = new MethodDefinition($"{name}Dummy", MethodAttributes.Public | MethodAttributes.Static,
            new MethodSignature(CallingConventionAttributes.Default, typeDefinition.Module!.CorLibTypeFactory.Boolean,
                new[]
                {
                    typeDefinition.Module!.CorLibTypeFactory.Int32, typeDefinition.Module!.CorLibTypeFactory.Int32
                }));
        dummyMethod.CilMethodBody = new CilMethodBody(dummyMethod)
        {
            Instructions =
            {
                { Ldc_I4_0 },
                { Ret }
            }
        };
        typeDefinition.Methods.Add(dummyMethod);
        var method = new MethodDefinition(name, MethodAttributes.Public | MethodAttributes.Static,
            new MethodSignature(CallingConventionAttributes.Default, typeDefinition.Module!.CorLibTypeFactory.Void,
                Enumerable.Empty<TypeSignature>()));
        method.CilMethodBody = new CilMethodBody(method)
        {
            Instructions =
            {
                { Ldc_I4_0 },
                { Ldc_I4_1 },
                { Ldftn, dummyMethod },
                { Calli,new StandAloneSignature(dummyMethod.Signature) },
                { Pop },
                { Ret }
            }
        };
        return method;
    }
}