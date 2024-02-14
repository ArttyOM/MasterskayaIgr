using UnityEngine;

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
    [SerializeField] private EnemyType[] enemies;

    SpriteRenderer spriteRenderer;

    public Sprite[] wallSprites;

    private void Start()
    {
        foreach (var enemy in enemies)
        {
            enemy.CalculateDamagePerSecond(healthPoints); 
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (var enemy in enemies)
        {
            if (collision.gameObject.tag == enemy.name)
            {
                healthPoints -= enemy.damagePerSecond * Time.deltaTime;
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
