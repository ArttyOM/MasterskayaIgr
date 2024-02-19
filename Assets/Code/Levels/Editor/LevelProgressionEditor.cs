using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Code.Levels.Editor
{
    [CustomEditor(typeof(LevelProgression))]
    public class LevelProgressionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var levelProgression = (LevelProgression)target;
            if (levelProgression == null) return;
            foreach (var level in levelProgression.Levels)
            {
                if (level.BuildIndex != -1) continue;
                if (!GUILayout.Button($"Add Scene {level.SceneName} to Build Settings")) continue;
                
                var scenes = EditorBuildSettings.scenes.ToList();
                scenes.Add(new EditorBuildSettingsScene(level.SceneName, true));
                EditorBuildSettings.scenes = scenes.ToArray();
            }
        }
    }
}