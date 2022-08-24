using System.ComponentModel.DataAnnotations;

namespace UseEveryOpCode.Setup;

public enum Runtime
{
    [Display(Name = ".NET 6")] DotNet6,
    [Display(Name = ".NET Core 3.1")] DotNetCore,
    [Display(Name = ".NET Framework 4.8")] DotNetFramework
}