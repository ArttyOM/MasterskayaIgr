using System.Collections.Generic;
using Code.Spells;
using UnityEngine;

namespace Code.HUD
{
    [CreateAssetMenu(menuName = "UI/Spell/Definition")]
    public class SpellDefinition : ScriptableObject
    {
        [SerializeField] private SpellType _spellType;
        [SerializeField] private string _title;
        [TextArea]
        [SerializeField] private string _shortDescription;
        [TextArea]
        [SerializeField] private string _fullDescription;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Sprite _guideArt;
        [SerializeField] private int _cost;
        [SerializeField] private List<Sprite> _targetItems;
        
        


        public string GetTitle() => _title;
        public string GetShortDescription() => _shortDescription;
        public string GetFullDescription() => _fullDescription;
        public Sprite GetIcon() => _icon;
        public Sprite GetGuideArt() => _guideArt;
        public int GetCost() => _cost;
        public List<Sprite> GetTargetItems() => _targetItems;
        public SpellType GetSpellType() => _spellType;
    }
}