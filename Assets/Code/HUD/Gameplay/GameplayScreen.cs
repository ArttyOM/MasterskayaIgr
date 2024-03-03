using System.Threading.Tasks;
using Code.Events;
using Code.Main;
using Code.PregameShop;
using Code.Saves;
using Code.Spells;
using Code.Upgrades;
using UnityEngine;

namespace Code.HUD.Gameplay
{
    public class GameplayScreen : MonoBehaviour
    {
        private ServiceLocator _services;
        [SerializeField] private SpellsPanel _spells;
        [SerializeField] private WalletView _wallet;

        private async void OnEnable()
        {
            while (ServiceLocator.Instance == null)
            {
                await Task.Yield();
            }
            _services = ServiceLocator.Instance;
            Initialize(_services.Profile, _services.SpellShop, _services.Events, new UpgradeService(_services.Profile.GetUpgrades(), _services.UpgradeSystem));
        }
        
        public void Initialize(PlayerProfile profile, SpellShop spellShop, InGameEvents events, UpgradeService upgradeService)
        {
            _spells.CreateButtons(profile.GetSpellBook(), events, upgradeService);
            _wallet.Render(profile.GetWallet(), _services.DropRewardsService);
        }
    }
}