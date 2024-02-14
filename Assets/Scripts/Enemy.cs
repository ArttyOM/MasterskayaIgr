using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable] 
public class MonsterType
{
    public string name; 
    public float hp;
    public float speed;
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private float monsterSpeed, monsterHP, depthScale = -0.1f;
    [SerializeField] private MonsterType[] monsterTypes; 

    Animator anim;

    private bool isDead;

    private void Start()
    {
        anim = GetComponent<Animator>();

        //foreach (MonsterType enemyType in monsterTypes)
        //{
        //    if (gameObject.CompareTag(enemyType.name))
        //    {
        //        monsterHP = enemyType.hp;
        //        monsterSpeed = enemyType.speed;
        //        break; 
        //    }
        //}

        MonsterType enemyType = EnemyManager.Instance.GetMonsterType(gameObject.name);
        if (enemyType != null)
        {
            monsterHP = enemyType.hp;
            monsterSpeed = enemyType.speed;
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = transform.position;
        Vector2 newPosition = position + Vector2.left * monsterSpeed * Time.fixedDeltaTime;
        GetComponent<Rigidbody2D>().MovePosition(newPosition);

        float newZ = transform.position.y * -depthScale;
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "TheWall")
            anim.SetBool("wallReached", true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "TheWall")
            anim.SetBool("wallReached", false);
    }

    private void GetDamage(float damage)
    {
        monsterHP -= damage;
        if (monsterHP <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true; 
        anim.SetTrigger("Die"); 
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
