using System;
using System.Collections.Generic;
using System.Linq;

namespace Code.Spells
{
    public class SpellBook
    {
        private readonly List<SpellType?> _selected;
        private readonly List<SpellType> _unlocked;
        private int _selectedSlots;

        public SpellBook(IEnumerable<SpellType?> selected, IEnumerable<SpellType> unlocked)
        {
            _selected = selected.ToList();
            _unlocked = unlocked.ToList();
        }

        public event Action Changed;

        public bool IsUnlocked(SpellType spell) => _unlocked.Contains(spell);

        public bool IsSelected(SpellType spell) => _selected.Contains(spell);
        

        public bool TrySelect(SpellType spell, int slot)
        {
            if (!IsUnlocked(spell)) return false;
            if (_selected.Contains(spell)) return false;
            if (slot >= _selected.Count) return false;
            _selected[slot] = spell;
            Changed?.Invoke();
            return true;
        }

        public bool TryDeselect(SpellType spell)
        {
            if (!IsUnlocked(spell)) return false;
            if (!_selected.Contains(spell)) return false;
            var index = _selected.IndexOf(spell);
            if (index < 0) return false;
            _selected[index] = null;
            Changed?.Invoke();
            return true;
        }
        public bool TryDeselect(int slot)
        {
            if (slot >= _selected.Count || slot < 0) return false;
            _selected[slot] = null;
            Changed?.Invoke();
            return true;
        }
        public IEnumerable<SpellType?> GetSelected() => _selected;
        public IEnumerable<SpellType> GetUnlocked() => _unlocked;

        public void TryUnlock(SpellType spell)
        {
            if (IsUnlocked(spell)) return;
            _unlocked.Add(spell);
            Changed?.Invoke();
        }

        public bool TrySelectInFirstEmpty(SpellType spell)
        {
            if (!IsUnlocked(spell)) return false;
            if (_selected.Contains(spell)) return false;
            var firstEmpty = -1;
            for (int i = 0; i < _selected.Count; i++)
            {
                if (_selected[i].HasValue) continue;
                firstEmpty = i;
            }
            if (firstEmpty < 0) return false;
            _selected[firstEmpty] = spell;
            Changed?.Invoke();
            return true;
        }

        public bool CanSelect(SpellType spellType)
        {
            if(IsSelected(spellType)) return false;
            var firstEmpty = -1;
            for (int i = 0; i < _selected.Count; i++)
            {
                if (_selected[i].HasValue) continue;
                firstEmpty = i;
            }
            if (firstEmpty < 0) return false;
            return true;
        }
    }
}