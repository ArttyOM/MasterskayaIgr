using System;
using System.Threading.Tasks;
using Code.DebugTools.Logger;
using Code.Main;
using Code.Spells;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD
{
    public class PrepareScreen : MonoBehaviour
    {
        [SerializeField] private SelectedSpellsView _selectedSpells;
        [SerializeField] private ShopSpellsView _shopSpellsView;
        [SerializeField] private WalletView _walletView;
        [SerializeField] private UpgradesListView _upgradesView;
        [SerializeField] private Button _startButton;
        
        private ServiceLocator _services;
        
        public async void OnEnable()
        {
            while (ServiceLocator.Instance == null)
            {
                await Task.Yield();
            }
            _services = ServiceLocator.Instance;
            _startButton.onClick.RemoveAllListeners();
            _startButton.onClick.AddListener(StartGame);
            _services.ShopSystem.Changed += Render;
            var spellBook = _services.Profile.GetSpellBook();
            spellBook.Changed += Render;
            Render();
        }

        private void StartGame()
        {
            ">>StartSession sending event: onStartSessionEvent".Colored(Color.gray).Log();
            _services.Events.OnSessionStart.OnNext(1);
        }
        private void Render()
        {
            _selectedSpells.Render(_services.Profile.GetSpellBook());
            _shopSpellsView.Render(_services.Profile.GetSpellBook(), _services.SpellShop, _services.ShopSystem);
            _upgradesView.Render(_services.ShopSystem);
            _walletView.Render(_services.Profile.GetWallet(), _services.DropRewardsService);
        }

        private void OnDisable()
        {
            if(_services != null)
                _services.ShopSystem.Changed -= Render;
        }
    }
}