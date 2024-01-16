using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD
{
    [RequireComponent(typeof(Button))]
    public class LevelSelectionStarter : MonoBehaviour
    {
        private Button _levelSelectionButton;

        public void Init(IObserver<Unit> onLevelSelection)
        {
            _levelSelectionButton = GetComponent<Button>();
            _levelSelectionButton.onClick.AddListener(() => onLevelSelection.OnNext(new Unit()));
        }
    }
}