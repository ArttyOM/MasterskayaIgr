using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Spells;
using UnityEngine;

namespace Code.PregameShop
{
    [CreateAssetMenu(menuName = "Shop/Spell Prices")]
    public class SpellShop : ScriptableObject
    {
        [SerializeField] private List<SpellShopInfo> _spells;

        public int GetCost(SpellType spellType)
        {
            foreach (var spellShopInfo in _spells)
            {
                if (spellShopInfo.Spell == spellType) return spellShopInfo.Cost;
            }
            return 0;
        }

        public Sprite GetSprite(SpellType spellType)
        {
            foreach (var spellShopInfo in _spells)
            {
                if (spellShopInfo.Spell == spellType) return spellShopInfo.Icon;
            }
            return null;
        }

        public IEnumerable<SpellType> GetSpells() => _spells.Select(s => s.Spell);
    }

    [Serializable]
    internal struct SpellShopInfo
    {
        public SpellType Spell;
        public int Cost;
        public Sprite Icon;
    }
}