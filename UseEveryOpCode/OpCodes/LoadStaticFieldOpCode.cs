using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace UseEveryOpCode.OpCodes;

public class LoadStaticFieldOpCode : IOpCode
{
    private readonly CilOpCode _opCode;
    public LoadStaticFieldOpCode(CilOpCode opCode)
    {
        _opCode = opCode;
    }
    public IList<CilInstruction> CallingInstructions => new List<CilInstruction>();
    public MethodDefinition? Generate(TypeDefinition typeDefinition)
    {
        var name = _opCode.ToString().Replace(".", "_");
        var dummyField =new FieldDefinition($"{name}Dummy", FieldAttributes.Public | FieldAttributes.Static, new FieldSignature(typeDefinition.Module.CorLibTypeFactory.Int32));
        typeDefinition.Fields.Add(dummyField);
        var method = new MethodDefinition(name, MethodAttributes.Public | MethodAttributes.Static,
            new MethodSignature(CallingConventionAttributes.Default, typeDefinition.Module!.CorLibTypeFactory.Void,
                Enumerable.Empty<TypeSignature>()));
        method.CilMethodBody = new CilMethodBody(method);
        method.CilMethodBody.Instructions.Add(_opCode,dummyField);
        method.CilMethodBody.Instructions.Add(CilOpCodes.Pop);
        method.CilMethodBody.Instructions.Add(CilOpCodes.Ret);
        return method;
    }
}