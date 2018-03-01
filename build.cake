#reference "tools/AutoCake.Build/tools/AutoCake.Build.dll"
#load      "tools/AutoCake.Build/content/build-tasks.cake"

BuildConfig.GeneratedAssemblyMetaDataReference = "BuildMetaDataAssemblyInfo.cs";
BuildConfig.AssemblyProperties["Test"] = "Value";
PublishActions.PackSettings.Symbols = true;
PublishActions.PackSettings.IncludeReferencedProjects = false;
PublishActions.PackSettings.Verbosity = NuGetVerbosity.Detailed;

XBuildHelper.NeverUseDotNetCore = true;

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
RunTarget(target);
