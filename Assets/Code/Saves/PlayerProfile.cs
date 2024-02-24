using System;
using System.Linq;
using Code.DebugTools.Logger;
using Code.Items;
using Code.Spells;
using Code.Upgrades;
using UnityEngine;

namespace Code.Saves
{
    public class PlayerProfile : StorageData<PlayerProfile.PlayerData>
    {
        [Serializable]
        public struct PlayerData
        {
            public int Level;
            public int LaunchCount;
            public int Coins;
            public string[] Upgrades;
            public int[] SelectedSpells;
            public SpellType[] UnlockedSpells;
        }

        private bool _isDirty;
        private readonly Wallet _wallet;
        private readonly UnitUpgrades _unitUpgrades;
        private readonly SpellBook _spellBook;
        public PlayerProfile(IPersistentStorage storage) : base(storage)
        {
            Initialize();
            _wallet = new Wallet(Data.Coins);
            _unitUpgrades = new UnitUpgrades(Data.Upgrades);
            _spellBook = new SpellBook(Data.SelectedSpells.Select(x => x != -1 ? (SpellType?)x : null), Data.UnlockedSpells);
            IncrementLaunchCount();
            Save();
        }

        protected override string StorageKey => nameof(PlayerProfile);
        protected override PlayerData CreateNew()
        {
            return new PlayerData()
            {
                Level = 0,
                LaunchCount = 0,
                Coins = 0,   
                SelectedSpells = new int[] { -1,-1,-1 },
                UnlockedSpells = new[] { SpellType.Badaboom , SpellType.Ice , SpellType.Mine },
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
            Data.SelectedSpells = _spellBook.GetSelected().Select(x => x.HasValue ? (int)x.Value : -1).ToArray();
            Data.Upgrades = _unitUpgrades.GetActiveUpgrades().ToArray();
        }

        public Wallet GetWallet() => _wallet;
        public SpellBook GetSpellBook() => _spellBook;
        public UnitUpgrades GetUpgrades() => _unitUpgrades;

        public int GetLaunchCount() => Data.LaunchCount;
    }

}