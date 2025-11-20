#addin nuget:?package=Cake.Unity&version=0.9.0
#addin nuget:?package=Cake.Discord&version=4.0.0

using System.Xml.Linq;

const string Clean = "Clean";
const string BuildAndroid = "Build-Android";
const string Deploy = "Deploy";
const string RunEditorTests = "Run-Editor-Tests";
const string RunTests = "Run-Tests";
const string RunPlayModeTests = "Run-PlayMode-Tests";
const string ArtifactsPath = @"./artifacts";

var targetTask = Argument("target", Deploy);

Task(Clean)
    .Does(() =>
{
    CleanDirectory(ArtifactsPath);
});

Task(BuildAndroid)
    .IsDependentOn(Clean)
    .IsDependentOn(RunTests)
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
    .Does(() =>
{
    var webHook = EnvironmentVariable("DISCORD_WEBHOOK");
    var artifactURL = EnvironmentVariable("ARTIFACT_PATH");
    var content = @"Hey😎! A build has been successfully builded and deployed to discord ✅. Take you tea and have a nice day!💖
    Btw, here is your derired build - " + artifactURL;
    DiscordChatProvider discordChatProvider = new DiscordChatProvider(Context);
    var urls = new List<string>() {
        "https://cdn.discordapp.com/attachments/1006168298171400374/1441133863492517908/35178619_021_1bca.jpg?ex=6920b000&is=691f5e80&hm=264b7fd10806b40b178991ac727ee1c166784400946a36cb6af36c85fb68e5f8&",
        "https://cdn.discordapp.com/attachments/1006168298171400374/1441133862871896084/61833642_003_5a78.jpg?ex=6920afff&is=691f5e7f&hm=caec12e20470d413132a0aa168c70ff5dfeb4419a0badf0727f9c51173beaa12&",
        "https://cdn.discordapp.com/attachments/1006168298171400374/1441133864431915138/download_1.jpg?ex=6920b000&is=691f5e80&hm=8802016a40df47b05134f1ed8c6703ab2f130431a8fdda8715b27182c8dd260e&",
        "https://cdn.discordapp.com/attachments/1006168298171400374/1441133864822112356/download.jpg?ex=6920b000&is=691f5e80&hm=050786ca1745fe486ffc55f6e44bf372b2feefeeb437a164dff13389b6eee3f5&",
        "https://cdn.discordapp.com/attachments/1006168298171400374/1441133865665302633/3407a720ee8913a6c437fd01e72705a2.jpg?ex=6920b000&is=691f5e80&hm=d804edd221dd81fac7f28555fb9dea055d3feb9693bb7ef299141725b5fbb6e4&",
        "https://cdn.discordapp.com/attachments/1006168298171400374/1441131860486000711/3407a720ee8913a6c437fd01e72705a2.jpg?ex=6920ae22&is=691f5ca2&hm=515532bf59e3c64980a26755cf84f4b7405c70d36dc4831c8658145b8cd9d847&"
    };
    
    var rnd = new System.Random();
    var url = urls[rnd.Next(0, urls.Count)];

    discordChatProvider.PostMessage(webHook, content, new DiscordChatMessageSettings() 
    {
        UserName = "Gojo Builder",
        ThrowOnFail = true,
        Tts = false,
        AvatarUrl = new Uri(url)
    });

    var discordProvider = new DiscordProvider(Context);
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