using UnityEngine;

namespace Code.Spells
{
    public class SpellExplosionPool: UniRx.Toolkit.ObjectPool<SpellExplosion>
    {
        public SpellExplosionPool(SpellExplosion prefab)
        {
            _prefab = prefab;
        }
        
        private SpellExplosion _prefab;

        protected override SpellExplosion CreateInstance()
        {
            return Object.Instantiate(_prefab);
        }

        public SpellExplosion Rent(Vector3 explosionDataWorldPosition, bool isMega)
        {
            var result = Rent();
            result.transform.position = explosionDataWorldPosition;
            result.isMega = isMega;
            result.VfxDestroyWithDelay();
            return result;
        }
    }
}