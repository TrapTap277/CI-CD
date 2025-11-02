using static Cake.Unity.Arguments.BuildTarget;

#addin nuget:?package=Cake.Unity&version=0.9.0

var target = Argument("target", "Build-Android");

Task("Clean-Artifacts")
    .Does(() =>
{
    CleanDirectory($"./artifacts");
});

Task("Build-Android")
    .IsDependentOn("Clean-Artifacts")
    .Does(() =>
    {
        var unityPath = @"C:\Program Files\Unity\Hub\Editor\6000.2.6f2\Editor\Unity.exe";

        UnityEditor(unityPath, new UnityEditorArguments() // use verbosity instead of unity path
        {
            ProjectPath = "CICDLearn",
            ExecuteMethod = "Editor.Builder.BuildAndroid",
            BuildTarget = Android
        });
    });


RunTarget(target);