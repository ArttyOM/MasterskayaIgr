using UnityEngine;

[System.Serializable]
public class EnemyStats
{
    public string name; 
    public float destroyInSec;

    public float hitPoints;
    public float moveSpeed;
    public int KillReward;
    
    [HideInInspector] public float damagePerSecond;

    public void CalculateDamagePerSecond(float wallHealth)
    {
        damagePerSecond = wallHealth / destroyInSec;
    }
    
}