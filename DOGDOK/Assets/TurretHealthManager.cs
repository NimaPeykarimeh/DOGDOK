using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretHealthManager : MonoBehaviour
{
    TurretController turretController;
    [SerializeField] int maxHealth = 200;
    [SerializeField] int currentHealth;
    public List<EnemyController> alertedEnemiesList;
    [Header("HealthBar")]
    [SerializeField] Transform healthBarCanvas;
    [SerializeField] Image healthBarImage;
    [SerializeField] float barSpeed = 0.5f;
    float targetValue;
    bool isReducinng = false;
    private Camera mainCam;
    private void Awake()
    {
        turretController = GetComponent<TurretController>();
        mainCam = Camera.main;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void GetDamage(int _damage)
    {
        currentHealth -= _damage;
        healthBarCanvas.gameObject.SetActive(currentHealth < maxHealth);
        targetValue = (float)((float)currentHealth / (float)maxHealth);
        //healthBarImage.fillAmount = (float)((float)currentHealth / (float)maxHealth);
        if (currentHealth <= 0)
        {
            DestroyTurret();
        }
    }

    private void Update()
    {
        if (healthBarCanvas.gameObject.activeSelf)
        {
            healthBarCanvas.LookAt(mainCam.transform.position);
            healthBarImage.fillAmount = Mathf.MoveTowards(healthBarImage.fillAmount, targetValue, barSpeed * Time.deltaTime); ;
        }
    }

    void DestroyTurret()
    {
        foreach (EnemyController _enemy in alertedEnemiesList)
        {
            _enemy.isTargetedTurret = false;
            _enemy.AlertEnemy(false,false,false,transform);
        }
        alertedEnemiesList.Clear();
        Destroy(gameObject);
    }
}
