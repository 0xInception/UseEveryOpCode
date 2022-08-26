using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace UseEveryOpCode.OpCodes;

public class SwitchOpCode : IOpCode
{
    public IList<CilInstruction> CallingInstructions => new List<CilInstruction>();
    public MethodDefinition? Generate(TypeDefinition typeDefinition)
    {
        var method = new MethodDefinition(CilOpCodes.Switch.ToString(), MethodAttributes.Public | MethodAttributes.Static,
            new MethodSignature(CallingConventionAttributes.Default, typeDefinition.Module!.CorLibTypeFactory.Void,
                Enumerable.Empty<TypeSignature>()));
        method.CilMethodBody = new CilMethodBody(method);
        var ret = new CilInstruction(CilOpCodes.Ret);
        var ldci4_0 = new CilInstruction(CilOpCodes.Ldc_I4_0);
        var ldci4_1 = new CilInstruction(CilOpCodes.Ldc_I4_1);

        method.CilMethodBody.Instructions.Add(CilOpCodes.Ldc_I4_0);
        method.CilMethodBody.Instructions.Add(new CilInstruction(CilOpCodes.Switch,new []{new CilInstructionLabel(ldci4_0),new CilInstructionLabel(ldci4_1),new CilInstructionLabel(ret)}));
        method.CilMethodBody.Instructions.Add(ldci4_0);
        method.CilMethodBody.Instructions.Add(CilOpCodes.Pop);
        method.CilMethodBody.Instructions.Add(ldci4_1);
        method.CilMethodBody.Instructions.Add(CilOpCodes.Pop);
         method.CilMethodBody.Instructions.Add(ret);
        method.CilMethodBody.Instructions.CalculateOffsets();
        return method;
    }
}