using System;
using System.Collections.Generic;
using System.Linq;
using Code.Saves;
using Code.Spells;
using Code.Upgrades;
using Unity.VisualScripting;

namespace Code.PregameShop
{
    public class ShopSystem
    {
        private const int MaxUpgradeCount = 4;
        private readonly PlayerProfile _player;
        private readonly SpellShop _spellShop;
        private readonly UpgradeSystem _upgradeSystem;

        public ShopSystem(PlayerProfile player, SpellShop spellShop, UpgradeSystem upgradeSystem)
        {
            _player = player;
            _spellShop = spellShop;
            _upgradeSystem = upgradeSystem;
        }


        public IEnumerable<UpgradeDefinition> GetUpgradeOffers()
        {
            var upgrades = _player.GetUpgrades();
            return _upgradeSystem.GetNextForUpgrade(upgrades).DistinctBy(x => x.GetTarget()).Take(MaxUpgradeCount);
        }

        public IEnumerable<SpellType> GetSpellOffers()
        {
            foreach (var spell in Enum.GetValues(typeof(SpellType)))
            {
                var spellType = (SpellType)spell;
                yield return spellType;
            }
        }


        public bool CanBuy(UpgradeDefinition upgrade) => _player.GetWallet().CanSpend(upgrade.GetCost());

        public void Buy(UpgradeDefinition upgradeDefinition)
        {
            if (_player.GetWallet().TrySpend(upgradeDefinition.GetCost()))
            {
                _player.GetUpgrades().TryUpgrade(upgradeDefinition.GetID());
            }
        }

        public bool CanBuy(SpellType spell) => _player.GetWallet().CanSpend(_spellShop.GetCost(spell));

        public void Buy(SpellType spell)
        {
            if (_player.GetWallet().TrySpend(_spellShop.GetCost(spell)))
            {
                _player.GetSpellBook().TryUnlock(spell);
            }
        }
    }
}