using System;
using System.Collections.Generic;
using Code.Projectiles;
using UniRx;
using UnityEngine;

namespace Code.Spells
{
    public class SpellsPanelActivator: IDisposable
    {
        public SpellsPanelActivator(IObservable<int> eventsSessionStart, IObservable<SpellType> eventsSpellSelected,
            IObservable<ExplosionData> onProjectileExploded)
        {
            _spellsPanel = GameObject.FindObjectOfType<SpellsPanel>(true);
            _spellsPanel.gameObject.SetActive(false);

            _spellButtons = _spellsPanel.GetComponentsInChildren<UISelectSpellButton>(true);
            
            _showSpellsPanelSubscription = eventsSessionStart
                .Subscribe(_ => _spellsPanel.gameObject.SetActive(true));

            _blockSpellsPanelSubscription = eventsSpellSelected
                .Subscribe(_ => SetSpellButtonsInteravctable(false));

            _unblockSpellsPanelSubscription = onProjectileExploded
                .Subscribe(_ => SetSpellButtonsInteravctable(true));
        }

        private readonly SpellsPanel _spellsPanel;
        private readonly IEnumerable<UISelectSpellButton> _spellButtons;
        private readonly IDisposable _showSpellsPanelSubscription;
        private readonly IDisposable _blockSpellsPanelSubscription;
        private readonly IDisposable _unblockSpellsPanelSubscription;
        
        public void Dispose()
        {
            _showSpellsPanelSubscription?.Dispose();
            _unblockSpellsPanelSubscription?.Dispose();
            _blockSpellsPanelSubscription?.Dispose();
        }

        private void SetSpellButtonsInteravctable(bool isInteractable)
        {
            foreach (var spellButton in _spellButtons)
            {
                spellButton.SetButtonInteractable(isInteractable);
            }
        }
    }
}