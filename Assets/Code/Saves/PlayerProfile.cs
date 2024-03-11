using System;
using System.Collections.Generic;
using System.Linq;
using Code.Items;
using Code.Spells;
using Code.Upgrades;

namespace Code.Saves
{
    public class PlayerProfile : StorageData<PlayerProfile.PlayerData>
    {
        private readonly DefaultPlayerProfile _defaultPlayerProfile;

        [Serializable]
        public struct PlayerData
        {
            public int Level;
            public int LaunchCount;
            public int Coins;
            public List<int> CompletedLevels;
            public string[] Upgrades;
            public SpellType[] SelectedSpells;
            public SpellType[] UnlockedSpells;
        }

        private bool _isDirty;
        private readonly Wallet _wallet;
        private readonly UnitUpgrades _unitUpgrades;
        private readonly SpellBook _spellBook;
        public PlayerProfile(IPersistentStorage storage, DefaultPlayerProfile defaultPlayerProfile) : base(storage)
        {
            _defaultPlayerProfile = defaultPlayerProfile;
            Initialize();
            _wallet = new Wallet(Data.Coins);
            _unitUpgrades = new UnitUpgrades(Data.Upgrades);
            _spellBook = new SpellBook(Data.SelectedSpells, Data.UnlockedSpells);
            IncrementLaunchCount();
            Save();
        }

        protected override string StorageKey => nameof(PlayerProfile);
        protected override PlayerData CreateNew()
        {
            return new PlayerData()
            {
                Level = _defaultPlayerProfile.GetStartLevel(),
                LaunchCount = 0,
                Coins = _defaultPlayerProfile.GetStartCoins(),   
                CompletedLevels = new List<int>(),
                SelectedSpells = _defaultPlayerProfile.GetSelectedSpells().Take(3).ToArray(),
                UnlockedSpells = _defaultPlayerProfile.GetUnlockedSpells().ToArray(),
                Upgrades = new string[]{},
            };
        }

        public bool IsFirstLaunch() => Data.LaunchCount == 1;

        public void IncrementLaunchCount()
        {
            Data.LaunchCount++;
        }

        
        protected override void BeforeSave()
        {
            Data.Coins = _wallet.Balance;
            Data.UnlockedSpells = _spellBook.GetUnlocked().ToArray();
            Data.SelectedSpells = _spellBook.GetSelected().ToArray();
            Data.Upgrades = _unitUpgrades.GetActiveUpgrades().ToArray();
        }

        public Wallet GetWallet() => _wallet;
        public SpellBook GetSpellBook() => _spellBook;
        public UnitUpgrades GetUpgrades() => _unitUpgrades;

        public int GetLaunchCount() => Data.LaunchCount;

        public void CompleteLevel(int level)
        {
            Data.CompletedLevels.Add(level);
        }

        public bool IsLevelCompleted(int level)
        {
            return Data.CompletedLevels.Contains(level);
        }

        public int SetCurrentLevel(int level) => Data.Level = level;
        public int GetCurrentLevel() => Data.Level;
    }

}