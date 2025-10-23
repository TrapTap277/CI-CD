using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Editor
{
    public class Builder
    {
        [MenuItem("Build/📦 Android")]
        public static void BuildAndroid()
        {
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                locationPathName = $"./artifacts/Build{Application.version}.apk",
                target = BuildTarget.Android,
                options = BuildOptions.None,
                scenes = new[] { "Assets/ThirdParty/FPS/Scenes/MainScene.unity" }
            });

            if(report.summary.result != BuildResult.Succeeded)
                throw new Exception("Build failed see log for details");
        }
    }
}