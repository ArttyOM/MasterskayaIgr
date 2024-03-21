using System;
using System.Threading.Tasks;
using Code.Main;
using Code.Upgrades;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class WallHealth : MonoBehaviour
{
    [SerializeField] private float healthPoints = 200f;
    [SerializeField] private EnemyStats[] enemyTypes;

    SpriteRenderer spriteRenderer;

    public Sprite[] wallSprites;

    private async void OnEnable()
    {
        
        while (ServiceLocator.Instance == null)
        {
            await Task.Yield();
        }
        var services = ServiceLocator.Instance;
        _subscription = services.Events.OnSessionStart.Subscribe(InitiateWall);
        InitiateWall(0);
    }

    private void InitiateWall(int _)
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

    private void OnDestroy()
    {
        _subscription.Dispose();
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

        if (healthPoints <= 0)
        {
            Debug.Log("Wall is destroyed");
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private float _lastHealth;
    private float _timer = .5f;
    private float _maxHealthPoints;
    private IDisposable _subscription;

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
        spriteRenderer.sprite = healthPoints switch
        {
            // Условия для изменения спрайта в зависимости от уровня HP
            <= 0 => wallSprites[3],
            <= 50 => wallSprites[2],
            <= 100 => wallSprites[2],
            <= 150 => wallSprites[1],
            _ => wallSprites[0]
        };
    }
}
