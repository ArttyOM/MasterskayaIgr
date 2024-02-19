using Code.Projectiles;
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