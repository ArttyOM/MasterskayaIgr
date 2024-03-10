using System.Collections.Generic;
using Code.Spells;
using UnityEngine;

namespace Code.HUD
{
    [CreateAssetMenu(menuName = "UI/Spell/DefinitionList")]
    public class SpellDefinitions : ScriptableObject
    {
        [SerializeField]
        private List<SpellDefinition> _spells = new();
        public SpellDefinition Get(SpellType spell)
        {
            for (int i = 0; i < _spells.Count; i++)
            {
                if (_spells[i].GetSpellType() == spell) return _spells[i];
            }
            return null;
        }

        public IEnumerable<SpellType> GetAll()
        {
            for (int i = 0; i < _spells.Count; i++)
            {
                yield return _spells[i].GetSpellType();
            }
        }
    }
}