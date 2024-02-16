using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Levels
{
    [CreateAssetMenu(menuName = "Levels/Progression")]
    public class LevelProgression : ScriptableObject
    {
        [SerializeField] private List<SceneReference> _levels;

        public IEnumerable<SceneReference> Levels => _levels;
        public SceneReference DefaultLevel => _levels[0];
    }

    [Serializable]
    public struct SceneReference : ISerializationCallbackReceiver
    {
        [HideInInspector]
        public string SceneName;
        [HideInInspector]
        public int BuildIndex;
#if UNITY_EDITOR
        public SceneAsset SceneAsset;
         
#endif

        public void OnBeforeSerialize()
        {
        #if UNITY_EDITOR
            if (SceneAsset != null)
            {
                SceneName = AssetDatabase.GetAssetPath(SceneAsset);
                BuildIndex = SceneUtility.GetBuildIndexByScenePath(SceneName);
            }
            else
            {
                SceneName = null;
                BuildIndex = -1;
            }
        #endif
        }

        public void OnAfterDeserialize()
        {
        }
    }
}