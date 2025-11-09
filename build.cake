#addin nuget:?package=Cake.Unity&version=0.9.0

const string Clean = "Clean";
const string BuildAndroid = "Build-Android";
const string ArtifactsPath = @"./artifacts";

var targetTask = Argument("target", BuildAndroid);

Task(Clean)
    .Does(() =>
{
    CleanDirectory(ArtifactsPath);
});

Task(BuildAndroid)
    .IsDependentOn(Clean)
    .Does(() =>
{
    var unityEditor = FindUnityEditor();

    UnityEditor(unityEditor.Path, new UnityEditorArguments() 
    {
        BuildTarget = BuildTarget.Android,
        ProjectPath = ".",
        ExecuteMethod = "Editor.Builder.BuildAndroid"
    });
});

RunTarget(targetTask);