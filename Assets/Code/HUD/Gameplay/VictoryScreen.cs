using System;
using Code.Main;
using UnityEngine;

namespace Code.HUD.Gameplay
{
    public class VictoryScreen : MonoBehaviour
    {
        [SerializeField] private RestartButton _restartButton;
        [SerializeField] private ToMenuButton _menuButton;
        

        private void Start()
        {
            var services = ServiceLocator.Instance;
            var level = services.LevelProgression.GetLevel(services.Profile.GetCurrentLevel());
            _restartButton.Init(services.Events.OnLevelRestart, level.BuildIndex);
            _menuButton.Init(services.Events.OnMenu, 0);
        }
    }
}