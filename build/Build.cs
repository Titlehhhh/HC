using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.MinVer;
using Nuke.Common.Tools.NerdbankGitVersioning;
using Nuke.Common.Tools.OctoVersion;
using Nuke.Common.Utilities.Collections;
using Serilog;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

   // [GitVersion(Framework = "net8.0")]
    //readonly GitVersion GitVersion;
    //[MinVer(Framework = "net8.0")]
   // readonly MinVer MinVer;

    [NerdbankGitVersioning]
    readonly NerdbankGitVersioning NBGV;
    
    
    
    [NuGetPackage("Microsoft.DotNet.ApiCompat.Tool", "Microsoft.DotNet.ApiCompat.Tool.dll", Framework = "net8.0")]
    Tool ApiCompatTool;
    
    [NuGetPackage("Microsoft.DotNet.GenAPI.Tool", "Microsoft.DotNet.GenAPI.Tool.dll", Framework = "net8.0")]
    Tool ApiGenTool;
    
    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    [Solution(GenerateProjects = true)] readonly Solution Solution;
    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(x =>
                x.SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
        });
    
    Target Print => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
           // Log.Information("GitVersion = {Value}", GitVersion.MajorMinorPatch);
          //  Log.Information("GitVersion.FullSemVer = {Value}", GitVersion.FullSemVer);
            
            
            
           // Log.Information("MinVer = {Value}", MinVer.Version);
            
            Log.Information("NerdbankVersioning = {Value}", NBGV.SimpleVersion);
        });

}
