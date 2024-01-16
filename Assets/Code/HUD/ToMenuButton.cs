using System;
using Code.DebugTools.Logger;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.HUD
{
    [DisallowMultipleComponent]
    public class ToMenuButton : MonoBehaviour
    {
        private Button restartButton;
        private IObserver<int> _onMenuEvent;

        private int _sceneIndexToPreload;

        public void Init(IObserver<int> onMenuEvent, int currentSceneIndex)
        {
            _sceneIndexToPreload = currentSceneIndex;

            _onMenuEvent = onMenuEvent;

            restartButton = gameObject.GetComponent<Button>();
            restartButton.onClick.AddListener(ToMenu);
        }

        private void ToMenu()
        {
            ">>ToMenu".Colored(Color.red).Log();
            _onMenuEvent.OnNext(_sceneIndexToPreload);
        }
    }
}