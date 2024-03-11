using System.Collections.Generic;
using UnityEngine;

namespace Code.HUD
{
    public class ModalsManager
    {
        private List<ModalScreen> _screens = new List<ModalScreen>();
        public void Add(ModalScreen screen)
        {
            _screens.Add(screen);
        }

        public void Remove(ModalScreen screen)
        {
            _screens.Remove(screen);
        }


        public void Activate(string modalScreen)
        {
            foreach (var screen in _screens)
            {
                if (screen.name == modalScreen)
                {
                    screen.gameObject.SetActive(true);
                }
                else
                {
                    screen.gameObject.SetActive(false);
                }
            }
        }

        public void Deactivate()
        {
            foreach (var screen in _screens)
            {
                screen.gameObject.SetActive(false);
            }
        }
    }
}