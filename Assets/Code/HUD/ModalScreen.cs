using Code.HUD.Start;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD
{
    public class ModalScreen : MonoBehaviour
    {
        [SerializeField] private Button _close;
        [SerializeField] private OverlayClose _overlayClose;
        

        private void Awake()
        {
            if (_close != null)
            {
                _close.onClick.RemoveAllListeners();
                _close.onClick.AddListener(Hide);
            }

            if (_overlayClose != null)
            {
                _overlayClose.Triggered += Hide;
            }
        }

        public void Init(ModalsManager manager)
        {
            _manager = manager;
            _manager.Add(this);
        }

        private ModalsManager _manager;
        public void Hide() => _manager.Deactivate();
        private void OnDestroy()
        {
            if(_manager!= null)
                _manager.Remove(this);
            if (_close != null)
            {
                _close.onClick.RemoveAllListeners();
            }

            if (_overlayClose != null)
            {
                _overlayClose.Triggered -= Hide;
            }
        }
    }
}