using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyAnimator : MonoBehaviour
{
    //[SerializeField] private float monsterSpeed, monsterHP, depthScale = -0.1f;
    //[SerializeField] private MonsterType[] monsterTypes; 

    Animator anim;

    private bool isDead;

    private void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    // private void FixedUpdate()
    // {
    //     Vector2 position = transform.position;
    //     Vector2 newPosition = position + Vector2.left * monsterSpeed * Time.fixedDeltaTime;
    //     GetComponent<Rigidbody2D>().MovePosition(newPosition);
    //
    //     float newZ = transform.position.y * -depthScale;
    //     transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    // }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "TheWall")
            anim.SetBool("wallReached", true);
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "TheWall")
            anim.SetBool("wallReached", false);
    }

    public void Die()
    {
        isDead = true; 
        anim.SetTrigger("Die"); 
    }
    
}
