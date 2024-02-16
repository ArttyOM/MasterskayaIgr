using Code.DebugTools.Logger;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class EnemyType
{
    public string name; 
    public float destroyInSec;
    [HideInInspector] public float damagePerSecond;

    public void CalculateDamagePerSecond(float wallHealth)
    {
        damagePerSecond = wallHealth / destroyInSec;
    }
}

public class WallHealth : MonoBehaviour
{
    [SerializeField] private float healthPoints = 200f;
    [SerializeField] private EnemyType[] enemyTypes;

    SpriteRenderer spriteRenderer;

    public Sprite[] wallSprites;

    private void Start()
    {
        foreach (var enemy in enemyTypes)
        {
            enemy.CalculateDamagePerSecond(healthPoints); 
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        foreach (var enemyType in enemyTypes)
        {
            if (collision.gameObject.tag == enemyType.name)
            {
                healthPoints -= enemyType.damagePerSecond * Time.deltaTime;
                $"healthPoints = {healthPoints}".Log();
                UpdateSprite();
                break;
            }
        }

        if (healthPoints <= 50)
        {
            Debug.Log("Wall is destroyed");
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private void UpdateSprite()
    {
        // Условия для изменения спрайта в зависимости от уровня HP
        if (healthPoints <= 50)
        {
            spriteRenderer.sprite = wallSprites[3]; // Спрайт для HP <= 50
        }
        else if (healthPoints <= 100)
        {
            spriteRenderer.sprite = wallSprites[2]; // Спрайт для HP <= 100
        }
        else if (healthPoints <= 150)
        {
            spriteRenderer.sprite = wallSprites[1]; // Спрайт для HP <= 150
        }
        else
        {
            spriteRenderer.sprite = wallSprites[0]; // Спрайт для полного HP
        }
    }
}
