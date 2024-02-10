using System.Collections.Generic;
using Code.Projectiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Spells
{
    [CreateAssetMenu(fileName = "BaseSpellsConfig", menuName = "Config/SpellsConfig", order = 1)]
    public class SpellsConfig: ScriptableObject
    { 
        [SerializeField] public List<SpellConfig> spellConfigs;
    }
}