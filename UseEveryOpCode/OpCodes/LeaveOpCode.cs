using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace UseEveryOpCode.OpCodes;
using static CilOpCodes;

public class LeaveOpCode : IOpCode
{
    private readonly CilOpCode _opCode;

    public LeaveOpCode(CilOpCode opCode)
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
        var ret = new CilInstruction(CilOpCodes.Ret);
        method.CilMethodBody = new CilMethodBody(method)
        {
            Instructions =
            {
                { Nop },
                { Nop },
                { Nop },
                { _opCode, ret.CreateLabel() },
                { Pop },
                { Nop },
                { Nop },
                { _opCode, ret.CreateLabel() },
                { ret }
            },
        };
        method.CilMethodBody.Instructions.CalculateOffsets();
        method.CilMethodBody.ExceptionHandlers.Add(new CilExceptionHandler()
        {
            TryStart = method.CilMethodBody.Instructions[1].CreateLabel(),
            TryEnd = method.CilMethodBody.Instructions[4].CreateLabel(),
            HandlerStart = method.CilMethodBody.Instructions[4].CreateLabel(),
            HandlerEnd = method.CilMethodBody.Instructions[8].CreateLabel(),
            HandlerType = CilExceptionHandlerType.Exception,
            ExceptionType = typeDefinition.Module.CorLibTypeFactory.Object.ToTypeDefOrRef()
        });
        return method;
    }
}