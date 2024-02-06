using System;
using Code.Projectiles;
using MyBox;
using UnityEngine;

namespace Code.Spells
{
    [Serializable]
    public struct SpellBalanceConfig
    {
        [SerializeField] public SpellExplosion spellExplosionVfxPrefab;
        
        [HideInInspector]public SpellType spellType;

        [Range(0f, 20f)]
        [SerializeField] public float radius;
        
        [SerializeField]
        [ConditionalField(nameof(spellType), false, SpellType.Poison, SpellType.Ice)]
        [Range(0f, 20f)]
        public float duration;
        
        [SerializeField]
        [ConditionalField(nameof(spellType), false, SpellType.Ice)]
        [Range(0f, 1f)]
        public float slowValue;

        [SerializeField]
        [Range(0f, 10000f)]
        public float damage;

        [SerializeField]
        [ConditionalField(nameof(spellType), false, SpellType.Poison)]
        [Range(0f, 10000f)]
        public float damagePerSecond;

        [SerializeField]
        [ConditionalField(nameof(spellType), false, SpellType.Mine, SpellType.LineAttack)]
        [Range(0f, 10000f)]
        public float throttleTimeOfExplosion;

        [SerializeField]
        [Range(0f, 1000f)]
        public float cooldown;
    }
}