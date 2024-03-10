using System;
using Code.DebugTools.Logger;
using Code.Main;
using Code.Upgrades;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class WallHealth : MonoBehaviour
{
    [SerializeField] private float healthPoints = 200f;
    [SerializeField] private EnemyStats[] enemyTypes;

    SpriteRenderer spriteRenderer;

    public Sprite[] wallSprites;

    private void Start()
    {
        var services = ServiceLocator.Instance;
        var newHp = services.UpgradeSystem.GetUpgradedValue(services.Profile.GetUpgrades(), UpgradeTarget.WallHp, healthPoints);
        foreach (var enemy in enemyTypes)
        {
            enemy.CalculateDamagePerSecond(healthPoints); 
        }
        healthPoints = newHp;
        _maxHealthPoints = newHp;
        _lastHealth = healthPoints;
        spriteRenderer = GetComponent<SpriteRenderer>();
        services.ChangeWallHp(newHp, newHp);
        UpdateSprite();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        foreach (var enemyType in enemyTypes)
        {
            //todo: This is a broken mechanic --- needs to be modified
            if (collision.gameObject.CompareTag(enemyType.name))
            {
                var damage = enemyType.damagePerSecond * Time.deltaTime;
                healthPoints -= damage;
                //$"healthPoints = {healthPoints}".Log();
                
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

    private float _lastHealth;
    private float _timer = .5f;
    private float _maxHealthPoints;

    private void FixedUpdate()
    {
        _timer -= Time.deltaTime;
        if (_timer > 0) return;
        _timer += .5f;
        if (_lastHealth > healthPoints)
        {
            ServiceLocator.Instance.DamageNumbers.Spawn(Mathf.CeilToInt(_lastHealth - healthPoints), transform.position);
            transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), .2f, 1, 1);
            ServiceLocator.Instance.ChangeWallHp(healthPoints, _maxHealthPoints);
            _lastHealth = healthPoints;
            
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
