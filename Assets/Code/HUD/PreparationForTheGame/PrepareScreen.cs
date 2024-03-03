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
        
        public void Start()
        {
            _services = ServiceLocator.Instance;
            _selectedSpells.SpellSelected += OnSpellSelected;
            _upgradesView.UpgradeBought += OnUpgradeBought;
            _startButton.onClick.RemoveAllListeners();
            _startButton.onClick.AddListener(StartGame);
            Render();
        }

        private void StartGame()
        {
            ">>StartSession sending event: onStartSessionEvent".Colored(Color.gray).Log();
            _services.Events.OnSessionStart.OnNext(1);
        }

        private void OnUpgradeBought()
        {
            _upgradesView.Render(_services.ShopSystem);
        }

        private void Render()
        {
            _selectedSpells.Render(_services.Profile.GetSpellBook());
            _shopSpellsView.Render(_services.Profile.GetSpellBook(), _services.SpellShop, _services.ShopSystem);
            _upgradesView.Render(_services.ShopSystem);
            _walletView.Render(_services.Profile.GetWallet(), _services.DropRewardsService);
        }

        private void OnSpellSelected(SpellType spell, int slotIndex)
        {
            var spellBook = _services.Profile.GetSpellBook();
            if (spellBook.TrySelect(spell, slotIndex))
            {
                Render();
            }
        }
    }
}