using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace UseEveryOpCode.OpCodes;

public class SetElementOpCode : IOpCode
{
    private readonly CilOpCode _opCode;

    public SetElementOpCode(CilOpCode opCode)
    {
        _opCode = opCode;
    }

    public IList<CilInstruction> CallingInstructions => new List<CilInstruction>();
    public MethodDefinition? Generate(TypeDefinition typeDefinition)
    {
        var method = new MethodDefinition(_opCode.ToString().Replace(".","_"), MethodAttributes.Public | MethodAttributes.Static,
            new MethodSignature(CallingConventionAttributes.Default, typeDefinition.Module!.CorLibTypeFactory.Void,
                Enumerable.Empty<TypeSignature>()));
        method.CilMethodBody = new CilMethodBody(method);
        method.CilMethodBody.Instructions.Add(CilOpCodes.Ldc_I4_1);
        method.CilMethodBody.Instructions.Add(CilOpCodes.Newarr,typeDefinition.Module.CorLibTypeFactory.Int32.ToTypeDefOrRef());
        method.CilMethodBody.Instructions.Add(CilOpCodes.Ldc_I4_0);
        method.CilMethodBody.Instructions.Add(CilOpCodes.Ldc_I4_0);
        method.CilMethodBody.Instructions.Add(_opCode);
        method.CilMethodBody.Instructions.Add(CilOpCodes.Ret);
        return method;
    }
}