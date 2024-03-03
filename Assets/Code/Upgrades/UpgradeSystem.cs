using System.Collections.Generic;
using System.Linq;
using Code.Items;
using UnityEngine;

namespace Code.Upgrades
{
    public class UpgradeSystem
    {
        private readonly List<UpgradeDefinition> _upgrades;

        public UpgradeSystem(IEnumerable<UpgradeDefinition> upgradeDefinitions) => _upgrades = upgradeDefinitions.ToList();


        public IEnumerable<UpgradeDefinition> GetNextForUpgrade(UnitUpgrades upgrades)
        {
            foreach (var upgrade in _upgrades)
            {
                if (upgrades.HasUpgrade(upgrade.GetID())) continue;
                yield return upgrade;
            }
        }


        public bool TryUpgrade(UpgradeDefinition upgrade, UnitUpgrades upgrades) => upgrades.TryUpgrade(upgrade.GetID());

        public float GetUpgradedValue(UnitUpgrades unitUpgrades, UpgradeTarget target, float baseValue)
        {
            foreach (var upgrade in _upgrades)
            {
                if (upgrade.GetTarget() != target) continue;
                if (!unitUpgrades.HasUpgrade(upgrade.GetID())) continue;
                var effect = upgrade.GetEffect();
                var effectType = upgrade.GetEffectType();
                switch (effectType)
                {
                    case EffectType.Add:
                        baseValue += effect;
                        break;
                    case EffectType.Multiply:
                        baseValue *= effect;
                        break;
                    case EffectType.Set:
                        baseValue = effect;
                        break;
                    default:
                        Debug.LogError($"Effect Type not found in upgrade {upgrade.name}");
                        break;
                }
            }
            return baseValue;
        }
    }
}