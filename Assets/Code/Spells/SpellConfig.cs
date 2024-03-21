using Code.Projectiles;
using MyBox;
using UnityEngine;

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
        
        [ConditionalField(true, nameof(IsSpellWithMega))]
        [SerializeField] public SpellBalanceConfig megaSpellBalance;
        private bool IsSpellWithMega() => megaCastWeaponType != ProjectileType.None;
        
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