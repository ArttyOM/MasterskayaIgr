using System.Collections.Generic;
using Code.Events;
using Code.HUD;
using Code.Upgrades;
using UnityEngine;

namespace Code.Spells
{
    public class SpellsPanel : MonoBehaviour
    {
        [SerializeField] private UISelectSpellButton _buttonPrefab;
        [SerializeField] private Transform _root;
        [SerializeField] private SpellsConfig _spellsConfig;
        
        [SerializeField] private List<UISelectSpellButton> _buttons = new();

        public void CreateButtons(SpellBook spellBook, SpellDefinitions spellDefinitions, InGameEvents events, UpgradeService upgradeService)
        {
            RemoveAllButtons();
            foreach (var selectedSpell in spellBook.GetSelected())
            {
                if (selectedSpell is SpellType.NoSpell) continue;
                var spellButton = Instantiate(_buttonPrefab, _root);
                spellButton.Init(spellDefinitions.Get(selectedSpell), events.OnSpellSelected, events.OnProjectileExploded, _spellsConfig.Get(selectedSpell), upgradeService);
                _buttons.Add(spellButton);
            }
        }

        private void RemoveAllButtons()
        {
            foreach (var button in _buttons)
            {
                Destroy(button.gameObject);
            }
            _buttons.Clear();
        }
    }
}