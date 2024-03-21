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
        [SerializeField] private List<LevelData> _levels;

        public IEnumerable<LevelData> Levels => _levels;
        public LevelData DefaultLevel => _levels[0];

        public LevelData GetLevel(int levelIndex)
        {
            if (levelIndex < 0 || levelIndex > _levels.Count - 1)
                return new LevelData() { SceneName = string.Empty, BuildIndex = -1 };
            return _levels[levelIndex];
        }

        public int GetNext(LevelData level)
        {
            var index = _levels.IndexOf(level);
            if (index < 0) return index;
            if (index == _levels.Count - 1) return index;
            return index + 1;
        }
    }

    [Serializable]
    public struct LevelData : ISerializationCallbackReceiver
    {
        public int CoinsReward;
        public string SceneName;
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