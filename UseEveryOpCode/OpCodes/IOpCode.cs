using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;

namespace UseEveryOpCode.OpCodes;

public interface IOpCode
{ 
    IList<CilInstruction> CallingInstructions { get; }
    MethodDefinition? Generate(TypeDefinition typeDefinition);
}