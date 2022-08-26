using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace UseEveryOpCode.OpCodes;

public class PopOpCode : IOpCode
{
    public IList<CilInstruction> CallingInstructions => new List<CilInstruction>();

    public MethodDefinition? Generate(TypeDefinition typeDefinition)
    {
        var method = new MethodDefinition(CilOpCodes.Pop.ToString(), MethodAttributes.Public | MethodAttributes.Static,
            new MethodSignature(CallingConventionAttributes.Default, typeDefinition.Module!.CorLibTypeFactory.Void,
                Enumerable.Empty<TypeSignature>()));
        method.CilMethodBody = new CilMethodBody(method);
        method.CilMethodBody.Instructions.Add(CilOpCodes.Ldnull);
        method.CilMethodBody.Instructions.Add(CilOpCodes.Pop);
        method.CilMethodBody.Instructions.Add(CilOpCodes.Ret);
        return method;

    }
}