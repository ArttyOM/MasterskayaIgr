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
            if (Main.ServiceLocator.Instance == null) return;
            _text.text = Main.ServiceLocator.Instance.Profile.GetLaunchCount().ToString();
        }
    }
}