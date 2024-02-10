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

        public SpellExplosion Rent(Vector3 explosionDataWorldPosition)
        {
            var result = Rent();
            result.transform.position = explosionDataWorldPosition;
            return result;
        }
    }
}