using System;
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
        public event Action<SpellType, int> SpellSelected;

        public void Render(SpellBook spellBook)
        {
            _spellBook = spellBook;
            RemoveAllSlots();
            var spells = _spellBook.GetSelected().ToArray();
            for (int i = 0; i < spells.Length; i++)
            {
                var slot = Instantiate(_prefab, transform);
                slot.OnSpellAssign += SpellAssigned;
                var spell = spells[i];
                if (!spell.HasValue)
                {
                    slot.RenderEmpty();
                }
                else
                {
                    slot.Render(spell.Value);    
                }
                _slots.Add(slot);
            }
        }

        private void SpellAssigned(SpellType spell, SpellSlotView view)
        {
            var index = _slots.IndexOf(view);
            if (index == -1) return;
            SpellSelected?.Invoke(spell, index);
        }


        private void OnDestroy() => RemoveAllSlots();

        private void RemoveAllSlots()
        {
            foreach (var slot in _slots)
            {
                slot.OnSpellAssign -= SpellAssigned;
                Destroy(slot.gameObject);
            }
            _slots.Clear();
        }
    }
}