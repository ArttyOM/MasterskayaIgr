using System;
using System.Collections.Generic;
using System.Linq;
using Code.HUD;
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
        private readonly SpellDefinitions _spellShop;
        private readonly UpgradeSystem _upgradeSystem;
        public event Action Changed;

        public ShopSystem(PlayerProfile player, SpellDefinitions spellShop, UpgradeSystem upgradeSystem)
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
            return _spellShop.GetAll();
        }


        public bool CanBuy(UpgradeDefinition upgrade) => _player.GetWallet().CanSpend(upgrade.GetCost());

        public void Buy(UpgradeDefinition upgradeDefinition)
        {
            if (_player.GetWallet().TrySpend(upgradeDefinition.GetCost()))
            {
                _player.GetUpgrades().TryUpgrade(upgradeDefinition.GetID());
                Changed?.Invoke();
            }
        }

        public bool CanBuy(SpellType spell) => _player.GetWallet().CanSpend(_spellShop.Get(spell).GetCost());

        public void Buy(SpellType spell)
        {
            if (_player.GetWallet().TrySpend(_spellShop.Get(spell).GetCost()))
            {
                _player.GetSpellBook().TryUnlock(spell);
                Changed?.Invoke();
            }
        }
    }
}