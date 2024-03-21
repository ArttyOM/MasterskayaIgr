using System;
using System.Threading.Tasks;
using Code.Events;
using Code.Main;
using Code.Saves;
using Code.Spells;
using Code.Upgrades;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD.Gameplay
{
    public class GameplayScreen : MonoBehaviour
    {
        private ServiceLocator _services;
        [SerializeField] private SpellsPanel _spells;
        [SerializeField] private WalletView _wallet;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _guideButton;
        [SerializeField] private HealthBar _healthBar;
        
        
        private async void OnEnable()
        {
            while (ServiceLocator.Instance == null)
            {
                await Task.Yield();
            }
            _services = ServiceLocator.Instance;
            _services.OnWallHpChanged += _healthBar.Render;
            Initialize(_services.Profile, _services.SpellShop, _services.Events, new UpgradeService(_services.Profile.GetUpgrades(), _services.UpgradeSystem));
        }

        private void OnDisable()
        {
            if(_services != null)
                _services.OnWallHpChanged -= _healthBar.Render;
        }

        public void Initialize(PlayerProfile profile, SpellDefinitions spellDefinitions, InGameEvents events, UpgradeService upgradeService)
        {
            _spells.CreateButtons(profile.GetSpellBook(), spellDefinitions, events, upgradeService);
            _wallet.Render(profile.GetWallet(), _services.DropRewardsService);
            _settingsButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.AddListener(() =>
            {
                events.OnSettingsRequested.OnNext(new Unit());
            });
            _guideButton.onClick.RemoveAllListeners();
            _guideButton.onClick.AddListener(() =>
            {
                events.OnGuideRequested.OnNext(new Unit());
            });
        }
    }
}