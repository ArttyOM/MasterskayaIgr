using System;
using Code.Main;
using TMPro;
using UnityEngine;

namespace Code.HUD
{
    public class LaunchCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private void Update()
        {
            if (MainEntryPoint.Instance == null) return;
            _text.text = MainEntryPoint.Instance.Profile.GetLaunchCount().ToString();
        }
    }
}