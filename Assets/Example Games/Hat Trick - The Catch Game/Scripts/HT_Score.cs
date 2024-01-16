using UnityEngine;
using System.Collections;

public class HT_Score : MonoBehaviour
{
    //public GUIText scoreText;
    public int ballValue;

    private int score;

    private void Start()
    {
        score = 0;
        UpdateScore();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        score += ballValue;
        UpdateScore();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bomb")
        {
            score -= ballValue * 2;
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        //scoreText.text = "SCORE:\n" + score;
    }
}