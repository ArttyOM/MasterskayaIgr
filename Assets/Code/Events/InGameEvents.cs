using System;
using Code.Enemies;
using Code.GameLoop;
using Code.Projectiles;
using Code.Spells;
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
        public readonly Subject<SpellType> OnSpellSelected = new();
        public readonly Subject<(Vector2Int, Vector3)> OnProjectileDestinationSelected =  new();
        
        public readonly Subject<ExplosionData> OnProjectileExploded =  new();
        public readonly Subject<(CommonEnemy, SpellExplosion)> OnExplosionEnter = new();
        public readonly Subject<CommonEnemy> OnEnemyDead = new();

        public void Dispose()
        {
            OnStartSimulation.Dispose();
        }
    }
}