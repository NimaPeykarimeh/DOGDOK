using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    PlayerController playerController;
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    [SerializeField] Image healthImage;
    [SerializeField] float UISpeed;
    [SerializeField] bool UIChanging;
    [SerializeField] int injuredLayerIndex;
    void Start()
    {
        currentHealth = maxHealth;
        playerController = GetComponent<PlayerController>();
    }

    public void ChangeHealth(int _changeAmount)
    {
        UIChanging = true;
        currentHealth += _changeAmount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            PlayerDead();
        }
        //healthImage.fillAmount = (float)currentHealth / maxHealth;
    }

    void PlayerDead()
    {
        Debug.Log("YouDead");
        currentHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (UIChanging)
        {
            healthImage.fillAmount = Mathf.MoveTowards(healthImage.fillAmount, (float)currentHealth / maxHealth,UISpeed * Time.deltaTime);
            //playerController.animator.SetLayerWeight(injuredLayerIndex,1 - (float)currentHealth / maxHealth);
            if (Mathf.Abs(healthImage.fillAmount - (float)currentHealth / maxHealth) <= 0.1f)
            {
                healthImage.fillAmount = (float)currentHealth / maxHealth;
                UIChanging = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeHealth(-5);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            ChangeHealth(5);
        }
    }
}
