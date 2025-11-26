#addin nuget:?package=Cake.Unity&version=0.9.0
#addin nuget:?package=Cake.Discord&version=4.0.0

using System.Xml.Linq;

const string Clean = "Clean";
const string BuildAndroid = "Build-Android";
const string Deploy = "Deploy";
const string RunEditorTests = "Run-Editor-Tests";
const string RunTests = "Run-Tests";
const string LintProject = "Lint-Project";
const string RunPlayModeTests = "Run-PlayMode-Tests";
const string ArtifactsPath = @"./artifacts";

var targetTask = Argument("target", LintProject);

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
        ExecuteMethod = "Editor.Builder.BuildAndroid",
        LogFile = "./artifacts/unity.log"
    }, new UnityEditorSettings() {RealTimeLog = true});
});

Task(Deploy)
    .Does(async  () =>
{
    var webHook = EnvironmentVariable("DISCORD_WEBHOOK");
    var artifactURL = EnvironmentVariable("ARTIFACT_PATH");
    var content = @"Hey😎! A build has been successfully builded and deployed to discord ✅. Take you tea and have a nice day!💖
    Btw, here is your derired build - " + artifactURL + "check it out";
    
    DiscordChatProvider discordChatProvider = new DiscordChatProvider(Context);

    var client = new System.Net.Http.HttpClient();
    var endpoint = "https://api.waifu.im/search?included_tags=waifu&gif=false&is_nsfw=true";
    var json = await client.GetStringAsync(endpoint);
    var result = System.Text.Json.JsonSerializer.Deserialize<WaifuResponse>(json);
    var imageUrl = result.images[0].url;
    var rnd = new System.Random();

    discordChatProvider.PostMessage(webHook, content, new DiscordChatMessageSettings() 
    {
        UserName = "Gojo Builder",
        AvatarUrl = new Uri(imageUrl)
    });
});

Task(LintProject)
    .Does(() =>
{
    Information("Running C# linters as part of the build process...");
    DotNetCoreBuild(projectFile);
});

Task(RunTests)
    .IsDependentOn(RunEditorTests)
    .IsDependentOn(RunPlayModeTests)
    .Does(() => 
{
    PrintTestResults("TestResults/EditorModeTests/test.xml");
    PrintTestResults("TestResults/PlayModeTests/test.xml");
});

Task(RunEditorTests)
    .Does(() =>
{
    StartProcess(
        @"C:\Program Files\Unity\Hub\Editor\6000.2.6f2\Editor\Unity.exe",
        "-runTests -batchMode -quit -logfile Logs/EditorMode.log -projectPath . -testResult ./TestResults/EditorModeTests/test.xml -testPlatform EditMode"
    );
});

Task(RunPlayModeTests)
    .Does(() =>
{
    StartProcess(
        @"C:\Program Files\Unity\Hub\Editor\6000.2.6f2\Editor\Unity.exe", 
        "-runTests -batchMode -quit -logfile Logs/PlayMode.log -projectPath . -testResult ./TestResults/PlayModeTests/test.xml -testPlatform PlayMode"
    );
});

RunTarget(targetTask);

void PrintTestResults(string path)
{
    if (!FileExists(path))
    {
        Warning($"Test results not found: {path}");
        return;
    }

    var xml = XDocument.Load(path);
    var run = xml.Root;

    var total = (int)run.Attribute("total");
    var passed = (int)run.Attribute("passed");
    var failed = (int)run.Attribute("failed");
    var skipped = (int)run.Attribute("skipped");

    Information("------------------------------");
    Information($"Results for: {path}");
    Information($"✔ Passed: {passed}");
    Information($"❌ Failed: {failed}");
    Information($"⏭ Skipped: {skipped}");
    Information("------------------------------");

    if (failed > 0)
        throw new Exception("Some tests failed.");
}

public class WaifuResponse
{
    public List<WaifuImage> images { get; set; }
}

public class WaifuImage
{
    public string url { get; set; }
}