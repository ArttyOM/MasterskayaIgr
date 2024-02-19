using System.Collections.Generic;
using System.Linq;

namespace Code.Upgrades
{
    public class UnitUpgrades
    {
        private readonly List<string> _upgrades;

        public UnitUpgrades(IEnumerable<string> upgrades)
        {
            _upgrades = upgrades.ToList();
        }

        public bool HasUpgrade(string upgradeID) => _upgrades.Contains(upgradeID);

        public bool TryUpgrade(string upgradeID)
        {
            if (HasUpgrade(upgradeID)) return false;
            _upgrades.Add(upgradeID);
            return true;
        }

        public void RemoveUpgrade(string upgradeID) => _upgrades.Remove(upgradeID);

        public IEnumerable<string> GetActiveUpgrades() => _upgrades;
    }
}