using Code.Main;
using Code.Spells;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.HUD
{
    public class DraggableSpell : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private SpellType _spellType;
        private RectTransform _draggable;

        public void SetSpell(SpellType spellType)
        {
            _spellType = spellType;
        }


        public void Disable()
        {
            enabled = false;
        }

        public void Enable()
        {
            enabled = true;
        }

        private Vector2 _startPosition;
        private Transform _startRoot;
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startPosition = transform.position;
            _startRoot = transform.parent;
            transform.SetParent(ServiceLocator.Instance.DragCanvas.transform);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(_startRoot);
            transform.position = _startPosition;
        }

        public SpellType GetSpellType() => _spellType;
    }
}