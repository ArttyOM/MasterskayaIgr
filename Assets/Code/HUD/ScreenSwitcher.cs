using System.Collections.Generic;
using Code.DebugTools.Logger;
using MasterServerToolkit.UI;
using UnityEngine;

namespace Code.HUD
{
    public class ScreenSwitcher
    {
        private Dictionary<ScreenType, UIView> _screens;

        public ScreenSwitcher()
        {
            ReInit();
        }

        public void ReInit()
        {
            "ReInit called".Colored(Color.red).Log();
            _screens = new Dictionary<ScreenType, UIView>();
            var screenViews = Object.FindObjectsOfType<ScreenView>();
            $"screenViews length is {screenViews.Length}".Log();
            foreach (var screen in screenViews) _screens.Add(screen.type, screen.GetComponent<UIView>());
            HideAllScreensInstantly();
        }

        public void ShowScreen(ScreenType screenType)
        {
            $">> ShowScreen {screenType}".Colored(Color.blue).Log();

            _screens[screenType].enabled = true;
            _screens[screenType].Show();
        }

        public void HideScreenInstantly(ScreenType screenType)
        {
            _screens[screenType].enabled = false;
            _screens[screenType].Hide(true);
        }

        public void HideAllScreensInstantly()
        {
            ">> HideAllScreensInstantly".Colored(Color.blue).Log();
            foreach (var screen in _screens)
            {
                screen.Value.enabled = false;
                screen.Value.Hide(true);
            }
        }
    }
}