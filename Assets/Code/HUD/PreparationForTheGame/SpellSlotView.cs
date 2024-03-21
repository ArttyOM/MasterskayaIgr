using System;
using Code.Spells;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.HUD
{
    public class SpellSlotView : MonoBehaviour, IDropHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _emptySprite;
        [SerializeField] private Button _removeButton;
        
        public event Action<SpellType, SpellSlotView> OnSpellAssign;
        public event Action<SpellSlotView> OnSpellRemoved;
        public void Render(SpellDefinition definition)
        {
            var icon = definition.GetIcon();
            _icon.sprite = icon;
            _removeButton.gameObject.SetActive(true);
            _removeButton.onClick.RemoveAllListeners();
            _removeButton.onClick.AddListener(() =>
            {
                OnSpellRemoved?.Invoke(this);    
            });
        }

        public void RenderEmpty()
        {
            _icon.sprite = _emptySprite;
            _removeButton.gameObject.SetActive(false);
            _removeButton.onClick.RemoveAllListeners();
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