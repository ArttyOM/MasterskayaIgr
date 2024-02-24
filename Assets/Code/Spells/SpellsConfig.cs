using System.Collections.Generic;
using UnityEngine;

namespace Code.Spells
{
    [CreateAssetMenu(fileName = "BaseSpellsConfig", menuName = "Config/SpellsConfig", order = 1)]
    public class SpellsConfig: ScriptableObject
    { 
        [SerializeField] public List<SpellConfig> spellConfigs;

        public SpellBalanceConfig Get(SpellType spellType)
        {
            for (int i = 0; i < spellConfigs.Count; i++)
            {
                if (spellConfigs[i].spellType == spellType) return spellConfigs[i].commonSpellBalance;
            }

            return default;
        }
    }
}