using UnityEngine;

//This script controls the explosion prefab
public class Explosion : MonoBehaviour
{
    public float lifeTime = 1f; //Lifetime of the explosion in seconds


    private void OnEnable()
    {
        //Invoke the Die method
        Invoke("Die", lifeTime);
    }

    private void OnDisable()
    {
        //Cancel the invoke if something else removes the explosion
        CancelInvoke("Die");
    }

    private void Die()
    {
        //Re-add the explosion to the pool
        ObjectPool.current.PoolObject(gameObject);
    }
}