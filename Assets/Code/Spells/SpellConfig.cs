using System.Runtime.Serialization;
using Code.Projectiles;
using Google.Protobuf.WellKnownTypes;
using MyBox;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Spells
{
    [System.Serializable]
    public struct SpellConfig : ISerializationCallbackReceiver
    {
        [SerializeField] public SpellType spellType;
        [SerializeField] public Spell spellPreparationVfxPrefab;
        
        //[SerializeField] public 
        [SerializeField] public ProjectileType megaCastWeaponType;

        [SerializeField] public SpellBalanceConfig commonSpellBalance;
        [SerializeField] public SpellBalanceConfig megaSpellBalance;

        public void OnBeforeSerialize()
        {
            commonSpellBalance.spellType = spellType;
            megaSpellBalance.spellType = spellType;
        }

        public void OnAfterDeserialize()
        {
           
        }
    }
}