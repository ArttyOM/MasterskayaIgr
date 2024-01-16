using MasterServerToolkit.MasterServer;
using MasterServerToolkit.Utils.Editor;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace MasterServerToolkit.Examples.BasicProfiles
{
    public class BasicProfilesBuild
    {
        [MenuItem(MstConstants.ToolMenu + "Build/Demos/Basic Profiles/All")]
        private static void BuildBoth()
        {
            BuildMasterForWindows();
            BuildClientForWindows();
        }

        [MenuItem(MstConstants.ToolMenu + "Build/Demos/Basic Profiles/Master Server")]
        private static void BuildMasterForWindows()
        {
            var buildFolder = Path.Combine("Builds", "BasicProfiles", "MasterServer");

            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[]
                    {"Assets/MasterServerToolkit/Demos/BasicProfiles/Scenes/MasterServer/MasterServer.unity"},
                locationPathName = Path.Combine(buildFolder, "MasterServer.exe"),
                target = BuildTarget.StandaloneWindows64,
#if UNITY_2021_1_OR_NEWER
                options = BuildOptions.ShowBuiltPlayer | BuildOptions.Development,
                subtarget = (int) StandaloneBuildSubtarget.Server
#else
                options = BuildOptions.EnableHeadlessMode | BuildOptions.ShowBuiltPlayer | BuildOptions.Development
#endif
            };

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            var summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                var appConfig = Mst.Args.AppConfigFile(buildFolder);

                var properties = new MstProperties();
                properties.Add(Mst.Args.Names.StartMaster, true);
                properties.Add(Mst.Args.Names.MasterIp, Mst.Args.MasterIp);
                properties.Add(Mst.Args.Names.MasterPort, Mst.Args.MasterPort);

                File.WriteAllText(appConfig, properties.ToReadableString("\n", "="));

                Debug.Log("Server build succeeded: " + summary.totalSize / 1024 + " kb");
            }

            if (summary.result == BuildResult.Failed) Debug.Log("Server build failed");
        }

        [MenuItem(MstConstants.ToolMenu + "Build/Demos/Basic Profiles/Client")]
        private static void BuildClientForWindows()
        {
            var buildFolder = Path.Combine("Builds", "BasicProfiles", "Client");

            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[] {"Assets/MasterServerToolkit/Demos/BasicProfiles/Scenes/Client/Client.unity"},
                locationPathName = Path.Combine(buildFolder, "Client.exe"),
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.ShowBuiltPlayer | BuildOptions.Development
            };

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            var summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                var appConfig = Mst.Args.AppConfigFile(buildFolder);

                var properties = new MstProperties();
                properties.Add(Mst.Args.Names.StartClientConnection, true);
                properties.Add(Mst.Args.Names.MasterIp, Mst.Args.MasterIp);
                properties.Add(Mst.Args.Names.MasterPort, Mst.Args.MasterPort);

                File.WriteAllText(appConfig, properties.ToReadableString("\n", "="));

                Debug.Log("Client build succeeded: " + summary.totalSize / 1024 + " kb");
            }

            if (summary.result == BuildResult.Failed) Debug.Log("Client build failed");
        }
    }
}