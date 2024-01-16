using System;
using System.Collections.Generic;
using System.Threading;
using Code.Pools;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Effects
{
    public static class ParticleSystemController
    {
        private static CancellationTokenSource _tokenSources;
        private static List<EffectView> effects;
        private static PoolCommonParent _poolCommonParent;
        private static Dictionary<EffectType, ObjectPool<ParticleSystem>> _pools;

        public static void Initialize(List<EffectView> effectsList, CancellationTokenSource tokenSource)
        {
            _tokenSources = tokenSource;
            effects = effectsList;
            _poolCommonParent = Object.FindObjectOfType<PoolCommonParent>();
            _pools = new Dictionary<EffectType, ObjectPool<ParticleSystem>>();

            CreatePoolEffects();
        }

        private static void CreatePoolEffects()
        {
            for (var i = 0; i < effects.Count; i++)
            {
                if (_pools.ContainsKey(effects[i].Type)) return;
                var particleEffect = effects[i].GetComponent<ParticleSystem>();
                var tempPool = new ObjectPool<ParticleSystem>(particleEffect, _poolCommonParent.transform, 5,
                    InitPool);
                _pools.Add(effects[i].Type, tempPool);
            }
        }

        private static void InitPool(ParticleSystem particleSystem)
        {
            particleSystem.Stop();
        }

        public static async void GetParticle(EffectType type, Vector3 position)
        {
            var pool = _pools[type];
            var activeParticle = pool.GetObject(position, Quaternion.identity);
            activeParticle.Stop();
            activeParticle.Clear();
            activeParticle.Play();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: _tokenSources.Token);
            pool.ReturnObject(activeParticle);
        }
    }
}