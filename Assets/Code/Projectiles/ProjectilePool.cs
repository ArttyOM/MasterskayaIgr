using UnityEngine;

namespace Code.Projectiles
{
    public class ProjectilePool: UniRx.Toolkit.ObjectPool<Projectile>
    {

        protected override Projectile CreateInstance()
        {
            var gameObject = new GameObject("Projectile");
            var instance = gameObject.AddComponent<Projectile>();
            return instance;
        }
    }
}