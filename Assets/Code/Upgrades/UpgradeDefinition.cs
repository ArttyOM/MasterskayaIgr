using UnityEngine;

namespace Code.Upgrades
{
    [CreateAssetMenu(menuName = "Upgrades/Upgrade")]
    public class UpgradeDefinition : ScriptableObject
    {
        [SerializeField] private string _title;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _cost;
        [SerializeField] private UpgradeTarget _target;
        [SerializeField] private float _effect;
        [SerializeField] private EffectType _effectType;

        public string GetID() => name;

        public UpgradeTarget GetTarget() => _target;
        public EffectType GetEffectType() => _effectType;
        public float GetEffect() => _effect;

        public int GetCost() => _cost;
        public Sprite GetIcon() => _icon;
        public string GetTitle() => _title;
        public string GetDescription() => _description;
    }



    public enum EffectType
    {
        Add,
        Multiply,
        Set
    }

    public enum UpgradeTarget
    {
        WallHp,
        TurretDuration,
        TurretDamage,
        TurretCooldown,
        SpellCooldown,
        SpellDamage,
    }
}