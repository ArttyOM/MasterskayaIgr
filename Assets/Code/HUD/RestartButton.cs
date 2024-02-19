using System;
using Code.DebugTools.Logger;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class RestartButton : MonoBehaviour
    {
        private Button restartButton;
        private IObserver<int> _onRestartEvent;

        private int _sceneIndexToRestart;

        public void Init(IObserver<int> onRestartEvent, int currentSceneIndex)
        {
            _sceneIndexToRestart = currentSceneIndex;

            _onRestartEvent = onRestartEvent;

            restartButton = gameObject.GetComponent<Button>();
            restartButton.onClick.AddListener(Restart);
        }

        private void Restart()
        {
            ">>Restart sending event: onRestartEvent".Colored(Color.gray).Log();
            _onRestartEvent.OnNext(_sceneIndexToRestart);
        }
    }
}