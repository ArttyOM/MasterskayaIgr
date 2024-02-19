using System.Collections.Generic;
using UnityEngine;

namespace Code.Enemies
{
    [CreateAssetMenu(fileName = "BaseEnemiesConfig", menuName = "Config/EnemiesConfig",  order = 1)]
    public class EnemiesConfig : ScriptableObject
    {
        public List<EnemyConfig> enemyConfigs;
    }
}