using System.Collections.Generic;
using Code.DebugTools.Logger;
using Code.Events;
using UniRx;
using UnityEngine;

namespace Code.HUD
{
    public class ScreenSwitcher
    {
        private Dictionary<ScreenType, List<ScreenView>> _screens;

        public ScreenSwitcher(InGameEvents events)
        {
            events.OnSessionStart.Subscribe(OnGameplayStart);
            ReInit();
        }


        private void OnGameplayStart(int x)
        {
            HideAllScreensInstantly();
            ShowScreen(ScreenType.Gameplay);
        }

        public void ReInit()
        {
            "ReInit called".Colored(Color.red).Log();
            _screens = new Dictionary<ScreenType, List<ScreenView>>();
            var screenViews = Object.FindObjectsOfType<ScreenView>(true);
            $"screenViews length is {screenViews.Length}".Log();
            AddScreens(screenViews);
            HideAllScreensInstantly();
        }

        private void AddScreens(ScreenView[] screenViews)
        {
            foreach (var screen in screenViews)
            {
                if (!_screens.ContainsKey(screen.type)) _screens.Add(screen.type, new List<ScreenView>());
                _screens[screen.type].Add(screen);
            }
        }

        public void ShowScreen(ScreenType screenType)
        {
            $">> ShowScreen {screenType}".Colored(Color.blue).Log();
            if (_screens.TryGetValue(screenType, out var screens))
            {
                foreach (var screen in screens)
                {
                    screen.enabled = true;
                    screen.Show();
                }    
            }
        }

        public void HideScreenInstantly(ScreenType screenType)
        {
            if (_screens.TryGetValue(screenType, out var screens))
            {
                foreach (var screen in screens)
                {
                    screen.enabled = false;
                    screen.Hide(true);
                }    
            }
        }

        public void HideAllScreensInstantly()
        {
            ">> HideAllScreensInstantly".Colored(Color.blue).Log();
            foreach (var screenList in _screens)
            {
                foreach (var screen in screenList.Value)
                {
                    screen.enabled = false;
                    screen.Hide(true);
                } 
            }
        }
    }
}