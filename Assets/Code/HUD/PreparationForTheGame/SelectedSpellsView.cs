using System.Collections.Generic;
using System.Linq;
using Code.Spells;
using UnityEngine;

namespace Code.HUD
{
    public class SelectedSpellsView : MonoBehaviour
    {
        [SerializeField] private SpellSlotView _prefab;
        private readonly List<SpellSlotView> _slots = new List<SpellSlotView>();
        private SpellBook _spellBook;
        private SpellDefinitions _spellDefinitions;

        public void Render(SpellBook spellBook, SpellDefinitions spellDefinitions)
        {
            _spellBook = spellBook;
            _spellDefinitions = spellDefinitions;
            RemoveAllSlots();
            var spells = _spellBook.GetSelected().ToArray();
            for (int i = 0; i < spells.Length; i++)
            {
                var slot = Instantiate(_prefab, transform);
                slot.OnSpellAssign += SpellAssigned;
                slot.OnSpellRemoved += SpellRemoved;
                var spell = spells[i];
                if (spell == SpellType.NoSpell)
                {
                    slot.RenderEmpty();
                }
                else
                {
                    slot.Render(_spellDefinitions.Get(spell));    
                }
                _slots.Add(slot);
            }
        }

        private void SpellRemoved(SpellSlotView view)
        {
            var index = _slots.IndexOf(view);
            if (index == -1) return;
            _spellBook.TryDeselect(index);
        }

        private void SpellAssigned(SpellType spell, SpellSlotView view)
        {
            var index = _slots.IndexOf(view);
            if (index == -1) return;
            _spellBook.TrySelect(spell, index);
        }


        private void OnDestroy() => RemoveAllSlots();

        private void RemoveAllSlots()
        {
            foreach (var slot in _slots)
            {
                slot.OnSpellAssign -= SpellAssigned;
                slot.OnSpellRemoved -= SpellRemoved;
                Destroy(slot.gameObject);
            }
            _slots.Clear();
        }
    }
}