using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] float spawnInterval;
    [SerializeField] float spawnTimer;
    [SerializeField] GameObject enemyPrefab;
    EnemySpawnArea enemySpawnArea;
    [SerializeField] int maxEnemyCount;
    [SerializeField] bool isSpawning;

    [Header("Player Distance")]
    [SerializeField] GameObject player;
    [SerializeField] float playerDistance;
    [SerializeField] float maxPlayerDistance;
    [SerializeField] float minPlayerDistance;

    [Header("Timer")]
    [SerializeField] float distanceTimer;
    [SerializeField] float distanceInterval;


    // Start is called before the first frame update
    void Start()
    {
        enemySpawnArea = GetComponent<EnemySpawnArea>();
        player = GameObject.FindGameObjectWithTag("Player");
        CreatePooler(maxEnemyCount);
    }

    public void BackToPooler(Transform _enemy)
    {
        _enemy.SetParent(transform);
    }

    private void CreatePooler(int count)
    {
        for (int i = 0; i < count; i++)
        {
            
            GameObject _spawnedEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
            _spawnedEnemy.transform.parent = transform;
            _spawnedEnemy.GetComponent<EnemyHealth>().enemySpawner = this;
            _spawnedEnemy.gameObject.SetActive(false);

        }
    }

    private void SpawnEnemy()
    {
        if (transform.childCount <= 0)
        {
            Debug.Log("Max Enemy");
        }
        else
        {
            GameObject _spawnedEnemy = transform.GetChild(0).gameObject;
            Vector3 _spawnArea = enemySpawnArea.spawnPosition;
            Vector3 _spawnSize = enemySpawnArea.spawnSize;

            float x = Random.Range(_spawnArea.x - (_spawnSize.x / 2), _spawnArea.x + (_spawnSize.x / 2));
            float z = Random.Range(_spawnArea.z - (_spawnSize.z / 2), _spawnArea.z + (_spawnSize.z / 2));
            float y = _spawnArea.y + _spawnSize.y / 2;

            Vector3 _randomPositon = new Vector3(x, y, z);
            _spawnedEnemy.transform.position = _randomPositon;

            _spawnedEnemy.SetActive(true);
            _spawnedEnemy.transform.parent = null;
        
            spawnTimer = 0;

        }
    }

    void CalculatePlayerDistance()//add one to each enemy
    {
        playerDistance = Vector3.Distance(transform.position,player.transform.position);
        isSpawning = playerDistance < maxPlayerDistance && playerDistance > minPlayerDistance;
        distanceTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        distanceTimer += Time.deltaTime;
        if (distanceTimer >= distanceInterval)
        {
            CalculatePlayerDistance();
        }

        if (isSpawning)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnEnemy();
            }
        }
    }
}
