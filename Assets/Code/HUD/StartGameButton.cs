using System;
using Code.DebugTools.Logger;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD
{
    [RequireComponent(typeof(Button))]
    public class StartGameButton : MonoBehaviour
    {
        private Button _startButton;

        private Action _action;


        public void Init(Action action)
        {
            _action = action;
            _startButton = gameObject.GetComponent<Button>();
            _startButton.onClick.AddListener(LaunchGame);
        }

        private void LaunchGame()
        {
            ">> LaunchGame".Log();
            _action?.Invoke();
        }
    }
}