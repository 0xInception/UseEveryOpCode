using UseEveryOpCode.Setup;

namespace UseEveryOpCode;

public class ResourceHelper
{
    private readonly Runtime _runtime;

    public ResourceHelper(Runtime runtime)
    {
        _runtime = runtime;
    }

    public byte[] ExtractModuleBytes()
    {
        var name = _runtime switch
        {
            Runtime.DotNet6 => "UseEveryOpCode.Resources._6._0.OpCodeTestApp.dll",
            Runtime.DotNetCore => "UseEveryOpCode.Resources._3._1.OpCodeTestApp.dll",
            Runtime.DotNetFramework => "UseEveryOpCode.Resources._4._8.OpCodeTestApp.exe",
            _ => throw new NotSupportedException("Could not locate module resource.")
        };

        return GetResource(name);
    }

    private byte[] GetResource(string name)
    {
        var source = typeof(ResourceHelper).Assembly.GetManifestResourceStream(name);
        using var memoryStream = new MemoryStream();
        source?.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }

    public void WriteRuntimes(string output, string name)
    {
        switch (_runtime)
        {
            case Runtime.DotNet6:
                var file1 = GetResource("UseEveryOpCode.Resources._6._0.OpCodeTestApp");
                var file2 = GetResource("UseEveryOpCode.Resources._6._0.OpCodeTestApp.exe");
                var file3 = GetResource("UseEveryOpCode.Resources._6._0.OpCodeTestApp.runtimeconfig.json");
                File.WriteAllBytes(Path.Combine(output, name), file1);
                File.WriteAllBytes(Path.Combine(output, name + ".exe"), file2);
                File.WriteAllBytes(Path.Combine(output, name + ".runtimeconfig.json"), file3);
                break;
            case Runtime.DotNetCore:
                var file4 = GetResource("UseEveryOpCode.Resources._3._1.OpCodeTestApp");
                var file5 = GetResource("UseEveryOpCode.Resources._3._1.OpCodeTestApp.exe");
                var file6 = GetResource("UseEveryOpCode.Resources._3._1.OpCodeTestApp.runtimeconfig.json");
                File.WriteAllBytes(Path.Combine(output, name), file4);
                File.WriteAllBytes(Path.Combine(output, name + ".exe"), file5);
                File.WriteAllBytes(Path.Combine(output, name + ".runtimeconfig.json"), file6);
                break;
        }
    }
}