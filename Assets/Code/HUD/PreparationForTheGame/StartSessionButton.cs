using System;
using Code.DebugTools.Logger;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class StartSessionButton : MonoBehaviour
    {
        private Button _startSessionButton;
        private IObserver<int> _onStartSessionEvent;

        public void Init(IObserver<int> onStartSessionEvent)
        {
            _onStartSessionEvent = onStartSessionEvent;

            _startSessionButton = gameObject.GetComponent<Button>();
            _startSessionButton.onClick.AddListener(StartSession);
        }

        private void StartSession()
        {
            ">>StartSession sending event: onStartSessionEvent".Colored(Color.gray).Log();
            _onStartSessionEvent.OnNext(1);
            // hide the button
            gameObject.SetActive(false);
        }
    }
}
