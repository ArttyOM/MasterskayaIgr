using UnityEngine;

namespace Code.Enemies
{
    public class CommonEnemyPool : UniRx.Toolkit.ObjectPool<CommonEnemy>
    {
        public CommonEnemyPool(CommonEnemy prefab)
        {
            _prefab = prefab;
        }
        
        private CommonEnemy _prefab;
        protected override CommonEnemy CreateInstance()
        {
            return GameObject.Instantiate(_prefab);
        }
    }
}