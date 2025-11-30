using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Editor
{
    public class Builder
    {
        private const string SceneName = "TelegramAPITestScene";

        [MenuItem("Build/📦 Android")]
        public static void BuildAndroid()
        {
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                locationPathName = $"./artifacts/Build{Application.version}.apk",
                target = BuildTarget.Android,
                options = BuildOptions.None,
                scenes = new[] { $"Assets/ThirdParty/FPS/Scenes/{SceneName}.unity" }
            });

            if(report.summary.result != BuildResult.Succeeded)
                throw new Exception("Build failed see log for details");
        }

        [MenuItem("Build/📦 WebGL")]
        public static void BuildWebGL()
        {
            var buildPath = $"./artifacts/WebGL_{Application.version}";

            if(!Directory.Exists(buildPath))
                Directory.CreateDirectory(buildPath);

            var buildOptions = new BuildPlayerOptions
            {
                locationPathName = buildPath,
                target = BuildTarget.WebGL,
                options = BuildOptions.None,
                scenes = new[] { $"Assets/ThirdParty/FPS/Scenes/{SceneName}.unity" }
            };

            var report = BuildPipeline.BuildPlayer(buildOptions);

            if(report.summary.result != BuildResult.Succeeded)
                throw new Exception("WebGL Build failed! See log for details.");

            Debug.Log($"WebGL Build succeeded! Output: {buildPath}");
        }
    }
}