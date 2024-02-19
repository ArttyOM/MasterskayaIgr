using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.HUD.Start
{
    public class OverlayClose : MonoBehaviour, IPointerClickHandler
    {
        public event Action Triggered;
        public void OnPointerClick(PointerEventData eventData)
        {
            Triggered?.Invoke();
        }
    }
}