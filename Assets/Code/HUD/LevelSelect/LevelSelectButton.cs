using Code.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD.LevelSelect
{
    public class LevelSelectButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _buttonIcon;
        
        [SerializeField] private Sprite _locked;
        [SerializeField] private Sprite _play;
        private LevelData _levelData;
        private InGameEvents _events;


        public void Init(LevelData levelData, InGameEvents events)
        {
            _levelData = levelData;
            _events = events;
            RefreshView(levelData);
        }

        private void RefreshView(LevelData levelData)
        {
            if (levelData.Locked)
            {
                _buttonIcon.sprite = _locked;
                _button.enabled = false;
            }
            else if (levelData.Name != string.Empty)
            {
                _buttonIcon.sprite = _play;
                _button.enabled = true;
                _button.onClick.AddListener(LoadLevel);
            }
        }

        private void OnDestroy() => _button.onClick.RemoveAllListeners();

        private void LoadLevel()
        {
            if (_levelData.Locked) return;
            _events.OnLevelStart.OnNext(_levelData.Index);
        }
    }
}