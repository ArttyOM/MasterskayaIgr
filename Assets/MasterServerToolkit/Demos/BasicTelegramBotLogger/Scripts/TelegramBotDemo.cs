using System;
using UnityEngine;

namespace MasterServerToolkit.Examples.BasicTelegramBotLogger
{
    public class TelegramBotDemo : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E)) throw new NullReferenceException("This object cannot be found");
        }
    }
}