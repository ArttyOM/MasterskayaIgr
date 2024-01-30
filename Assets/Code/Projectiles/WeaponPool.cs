using Code.Enemies;
using UnityEngine;

namespace Code.Projectiles
{
    public class WeaponPool: UniRx.Toolkit.ObjectPool<Weapon>
    {
        public WeaponPool(Weapon prefab)
        {
            _prefab = prefab;
        }
        
        private Weapon _prefab;

        protected override Weapon CreateInstance()
        {
            return Object.Instantiate(_prefab);
        }
    }
}