using System;
using Code.GameLoop;
using UniRx;
using UnityEngine;

namespace Code.Events
{
    public class InGameEvents : IDisposable
    {
        public readonly Subject<Unit> OnStartSimulation = new();

        public readonly Subject<int> OnLevelRestart = new();
        public readonly Subject<int> OnMenu = new();
        public readonly Subject<int> OnLevelStart = new();
        public readonly Subject<Unit> OnLevelSelection = new();
        // Even for start game session after preparation - run enemies waves
        public readonly Subject<int> OnSessionStart = new();

        public readonly Subject<LevelEndResult> OnLevelEnd = new();


        public void Dispose()
        {
            OnStartSimulation.Dispose();
        }
    }
}