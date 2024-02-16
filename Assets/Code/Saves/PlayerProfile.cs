using System;
using UnityEngine;

namespace Code.Saves
{
    public class PlayerProfile : StorageData<PlayerData>
    {
        public PlayerProfile(IPersistentStorage storage) : base(storage) => Initialize();
        protected override string StorageKey => nameof(PlayerProfile);
        protected override PlayerData CreateNew()
        {
            return new PlayerData()
            {
                Level = 0,
                LaunchCount = 0
            };
        }

        public bool IsFirstLaunch() => Data.LaunchCount == 0;

        public void IncrementLaunchCount()
        {
            Data.LaunchCount++;
            Save();
        }

        public int GetLaunchCount() => Data.LaunchCount;

        
    }

    [Serializable]
    public struct PlayerData
    {
        public int Level;
        public int LaunchCount;
    }
}