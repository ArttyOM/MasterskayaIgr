using Code.Spells;
using UnityEngine;

namespace Code.Projectiles
{
    public struct ExplosionData
    {
        
        public ExplosionData(ProjectileType projectileType, SpellType spellType, Vector3 worldPosition, Vector2Int gridCoords)
        {
            GetProjectileType = projectileType;
            GetSpellType = spellType;
            GetWorldPosition = worldPosition;
            GetGridCoords = gridCoords;
        }

        public Vector3 GetWorldPosition { get; }
        public SpellType GetSpellType { get; }
        public ProjectileType GetProjectileType { get; }

        public Vector2Int GetGridCoords { get; }
    }
}