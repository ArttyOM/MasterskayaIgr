using System;
using Code.PregameShop;
using Code.Spells;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.HUD
{
    public class SpellSlotView : MonoBehaviour, IDropHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private SpellShop _spellShop;
        [SerializeField] private Sprite _emptySprite;
        public event Action<SpellType, SpellSlotView> OnSpellAssign;
        public void Render(SpellType spell)
        {
            var icon = _spellShop.GetSprite(spell);
            _icon.sprite = icon;
        }

        public void RenderEmpty()
        {
            _icon.sprite = _emptySprite;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return;
            if (eventData.pointerDrag.TryGetComponent<DraggableSpell>(out var spell))
            {
                OnSpellAssign?.Invoke(spell.GetSpellType(), this);
            }
        }
    }
}