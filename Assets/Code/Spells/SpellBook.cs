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
        public bool IsUnlocked(SpellType spell) => _unlocked.Contains(spell);

        public bool IsSelected(SpellType spell) => _selected.Contains(spell);
        

        public bool TrySelect(SpellType spell, int slot)
        {
            if (!IsUnlocked(spell)) return false;
            if (_selected.Contains(spell)) return false;
            if (slot >= _selected.Count) return false;
            _selected[slot] = spell;
            return true;
        }

        public bool TryDeselect(SpellType spell)
        {
            if (!IsUnlocked(spell)) return false;
            if (_selected.Contains(spell)) return false;
            _selected.Remove(spell);
            return true;
        }
        public bool TryDeselect(int slot)
        {
            if (slot >= _selected.Count || slot < 0) return false;
            _selected.RemoveAt(slot);
            return true;
        }
        public IEnumerable<SpellType?> GetSelected() => _selected;
        public IEnumerable<SpellType> GetUnlocked() => _unlocked;

        public void TryUnlock(SpellType spell)
        {
            if (IsUnlocked(spell)) return;
            _unlocked.Add(spell);
        }
    }
}