using UnityEngine;

namespace Code.Projectiles
{
    public class Weapon:MonoBehaviour
    {
        [SerializeField] private ProjectileType _projectileType;

        public ProjectileType GetProjectileType => _projectileType;
    }
}