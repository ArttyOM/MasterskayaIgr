using Code.Main;
using UnityEngine;

namespace Code.HUD
{
    public class CheatAddMoney : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                ServiceLocator.Instance.Profile.GetWallet().Add(100);
            }
        }
    }
}