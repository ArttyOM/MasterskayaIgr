using Code.DebugTools.Logger;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD
{
    [RequireComponent(typeof(Button))]
    public class StartGameButton : MonoBehaviour
    {
        private Button _startButton;

        private ScreenSwitcher _screenSwitcher;


        public void Init(ScreenSwitcher screenSwitcher)
        {
            _screenSwitcher = screenSwitcher;
            _startButton = gameObject.GetComponent<Button>();
            _startButton.onClick.AddListener(LaunchGame);
        }

        private void LaunchGame()
        {
            ">> LaunchGame".Log();
            _screenSwitcher.HideAllScreensInstantly();
        }
    }
}