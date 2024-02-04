using UnityEngine;

namespace Code.Projectiles
{
    [System.Serializable]
    public struct WeaponSpawnPriorityPair
    {
        [SerializeField] public Weapon weaponPrefab;
        [Range(0, 200)][SerializeField] public int priority;
    }
}