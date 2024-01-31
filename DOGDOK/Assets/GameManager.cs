using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int maxEnemyInScene;
    public float enemySpawnCountMultiplier;

    [Header("difficultySettings")]
    [SerializeField] int addedMaxEnemyPerDay;

    ScoreManager scoreManager;

    [SerializeField] float maxSpawnInterval;
    [SerializeField] float minSpawnInterval;
    [SerializeField] float gameDuration;
    [SerializeField] float gameTimer;
    [SerializeField] float currentSpawnInterval;

    [SerializeField] List<EnemySpawner> spawners;

    [SerializeField] TextMeshProUGUI FPSText;

    public List<EnemyController> enemyControllerList;
    List<EnemyController> nonAlertedEnemies;
    List<EnemyController> alertedEnemies;

    float timer;
    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        foreach (EnemyController _enemy in enemyControllerList)
        {
            nonAlertedEnemies.Add(_enemy);
        }
    }

    public void ChangeDifficulty()
    {
        maxEnemyInScene += addedMaxEnemyPerDay;
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        scoreManager.ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void AlertEnemy()
    {
        int _randomEnemy = Random.Range(0,nonAlertedEnemies.Count);
        //nonAlertedEnemies[_randomEnemy].AlertEnemy();
    }


    private void Update()
    {
        gameTimer += Time.deltaTime;
        currentSpawnInterval = Mathf.Lerp(maxSpawnInterval,minSpawnInterval,gameTimer / gameDuration);
        foreach (EnemySpawner _spawner in spawners)
        {
            _spawner.spawnInterval = currentSpawnInterval;
        }
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            FPSText.text = (1 / Time.deltaTime).ToString("0");
            timer = 0.3f;
        }
    }
}
