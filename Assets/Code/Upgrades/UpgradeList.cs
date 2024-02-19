using System.Collections.Generic;
using UnityEngine;

namespace Code.Upgrades
{
    [CreateAssetMenu(menuName = "Upgrades/UpgradeList")]
    public class UpgradeList : ScriptableObject
    {
        [SerializeField] private List<UpgradeDefinition> _upgrades;

        public IEnumerable<UpgradeDefinition> Upgrades => _upgrades;
    }
}