using System.Collections.Generic;
using UnityEngine;

namespace Code.Upgrades
{
    public class UpgradeService : MonoBehaviour
    {
        private UpgradeSystem _system;
        [SerializeField] private List<UpgradeDefinition> _upgradeDefinitions;

        public void Initialize()
        {
            _system = new UpgradeSystem(_upgradeDefinitions);
        }
        
        public float GetUpgradedValue(UnitUpgrades unitUpgrades, UpgradeTarget target, float baseValue) => _system.GetUpgradedValue(unitUpgrades, target, baseValue);
        
    }
}