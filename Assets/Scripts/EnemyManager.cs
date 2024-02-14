using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    public MonsterType[] monsterTypes;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Опционально, если вы хотите, чтобы менеджер сохранялся между сценами
        }
    }

    public MonsterType GetMonsterType(string name)
    {
        foreach (var type in monsterTypes)
        {
            if (type.name == name)
                return type;
        }
        return null; // или выбросить исключение, если тип не найден
    }
}
