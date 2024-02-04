using UnityEngine;

namespace Code.Spells
{
    public class Spell:MonoBehaviour
    {
        [SerializeField] private SpellType _spellType;
        public SpellType GetSpellType => _spellType;
    }
}