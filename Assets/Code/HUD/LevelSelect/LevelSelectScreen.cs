using System.Collections.Generic;
using Code.Events;
using Code.Levels;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD.LevelSelect
{
    public class LevelSelectScreen : MonoBehaviour
    {
        [SerializeField] private LevelSelectButton _prefab;
        [SerializeField] private Transform _buttonsRoot;
        [SerializeField] private bool _showLocked;
        [SerializeField] private int _length;
        [SerializeField] private List<LevelSelectButton> _buttons;
        [SerializeField] private Button _backButton;
        

        public void Init(LevelProgression levelProgression, InGameEvents events, ScreenSwitcher switcher)
        {
            _backButton.onClick.AddListener(() =>
            {
                events.OnMenu.OnNext(0);
            });
            foreach (var level in levelProgression.Levels)
            {
                var button = GameObject.Instantiate(_prefab, _buttonsRoot);
                button.Init(new LevelData()
                {
                    Index = level.BuildIndex,
                    Name = level.SceneName,
                    Locked = false
                }, events);
                _buttons.Add(button);
            }

            if (!_showLocked || _buttons.Count >= _length) return;

            while (_buttons.Count < _length)
            {
                var button = GameObject.Instantiate(_prefab, _buttonsRoot);
                button.Init(new LevelData()
                {
                    Index = -1,
                    Name = string.Empty,
                    Locked = true
                }, events);
                _buttons.Add(button);
            }
        }
    }
}