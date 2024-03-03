using System.Collections.Generic;
using Code.Spells;
using UnityEngine;

namespace Code.Saves
{
    [CreateAssetMenu(menuName = "PlayerProfile/Default")] 
    public class DefaultPlayerProfile : ScriptableObject
    {
        [SerializeField] private int _StartLevel;
        [SerializeField] private int _Coins;
        [SerializeField] private List<SpellType> _unlockedSpells;
        [SerializeField] private List<SpellType> _selectedSpells;

        public int GetStartLevel() => _StartLevel;
        public int GetStartCoins() => _Coins;
        public IEnumerable<SpellType> GetUnlockedSpells() => _unlockedSpells;
        public IEnumerable<SpellType> GetSelectedSpells() => _selectedSpells;
    }
}