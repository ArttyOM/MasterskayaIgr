namespace Code.Upgrades
{
    public class UpgradeService
    {
        private readonly UnitUpgrades _unitUpgrades;
        private readonly UpgradeSystem _upgradeSystem;

        public UpgradeService(UnitUpgrades unitUpgrades, UpgradeSystem upgradeSystem)
        {
            _unitUpgrades = unitUpgrades;
            _upgradeSystem = upgradeSystem;
        }
        public float GetUpgradedValue(UpgradeTarget target, float baseValue) => _upgradeSystem.GetUpgradedValue(_unitUpgrades, target, baseValue);
        
    }
}