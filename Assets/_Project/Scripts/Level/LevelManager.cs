using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private EnemyAI enemy;
    [SerializeField] private float enemyBornDuration = 10f;
    [SerializeField] private int maxEnemy = 5;
    private EnemyAI[] enemies = new EnemyAI[10];
    [SerializeField] private Transform[] bornPlaces;
    [SerializeField] private Transform player;
    private float curEnemyDelay = 10f;
    private int curEnemy = 0;
    private void Awake()
    {
        for(int i = 0; i < 10; i++)
        {
            enemies[i] = Instantiate(enemy);
            enemies[i].Init(bornPlaces, player);
            enemies[i].gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        for (int i = 0; i < 10; i++) enemies[i].EnemyDead += EnemyDead;
    }
    private void OnDisable()
    {
        for (int i = 0; i < 10; i++) enemies[i].EnemyDead -= EnemyDead;
    }
    private void Update()
    {
        if (curEnemy >= maxEnemy) return;
        curEnemyDelay += Time.deltaTime;
        if (curEnemyDelay >= enemyBornDuration)
            TryBornEnemy();
    }
    private void TryBornEnemy()
    {
        curEnemyDelay = 0f;
        for(int i = 0; i < 10; i++)
        {
            if (!enemies[i].gameObject.activeSelf)
            {
                enemies[i].gameObject.SetActive(true);
                curEnemy++;
                return;
            }
        }
    }
    private void EnemyDead() => curEnemy = Mathf.Max(0, curEnemy - 1);
}
