using Code.Projectiles;
using UnityEngine;

namespace Code.Spells
{
    [System.Serializable]
    public struct SpellConfig
    {
        [SerializeField] public SpellType spellType;
        [SerializeField] public Spell spellVfxPrefab;
        [SerializeField] ProjectileType megaCastWeaponType;
    }
}