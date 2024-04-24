using System;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using Code.DebugTools.Logger;
using Code.Main;
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
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(Restart);
        }

        private void Restart()
        {
            ">>Restart sending event: onRestartEvent".Colored(Color.gray).Log();

            if(Appodeal.isLoaded(Appodeal.INTERSTITIAL)) {
                Appodeal.show(Appodeal.INTERSTITIAL);
            }
            _onRestartEvent.OnNext(_sceneIndexToRestart);
        }
        
    }
}