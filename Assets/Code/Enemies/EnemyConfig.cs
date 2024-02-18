using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Enemies
{
    [System.Serializable]
    public struct EnemyConfig
    {
        [SerializeField] public EnemyType enemyType;
        [SerializeField] public CommonEnemy prefab;
        [SerializeField] public EnemyStats enemyStats;
    }
}