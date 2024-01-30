using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Projectiles
{

    [CreateAssetMenu(fileName = "BaseWeaponConfig", menuName = "Config/WeaponRanfomPriorityConfig", order = 1)]
    public class WeaponSpawnChanceConfig: ScriptableObject
    {
        /// <summary>
        /// prioriy = 0 - не спаунить совсем
        /// чем выше prioriy, тем выше вероятность спауна по сравнению с остальными айтемами 
        /// </summary>
        [FormerlySerializedAs("weaponPriorityPair")]
        [Tooltip("чем выше prioriy, тем выше вероятность спауна по сравнению с остальными айтемами")]
        [SerializeField] public List<WeaponSpawnPriorityPair> weaponPriorityPairs;
    }
}