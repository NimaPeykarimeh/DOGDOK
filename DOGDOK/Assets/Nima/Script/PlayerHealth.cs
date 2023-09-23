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
    [Header("HitAnimation")]
    [SerializeField] float animationDuration;
    [SerializeField] float hitAnimTimer;
    [SerializeField] int animationHitLayer;
    [SerializeField] float animationWeight;
    [SerializeField] bool isGettingHit;
    [SerializeField] bool hitFedeIn;
    [SerializeField] float hitDelayTimer = 0.3f;

    void Start()
    {
        currentHealth = maxHealth;
        playerController = GetComponent<PlayerController>();
    }

    public void ChangeHealth(int _changeAmount)
    {
        UIChanging = true;
        currentHealth += _changeAmount;
        if (_changeAmount < 0 && !isGettingHit)//hit animation
        {
            playerController.animator.SetTrigger("GetHit");
            playerController.characterMovement.ToggleRunState(CharacterMovement.MoveStates.Crouched);
            hitAnimTimer = animationDuration;
            hitFedeIn = true;
            isGettingHit = true;
            animationWeight = 0;
            hitDelayTimer = 0.75f;
        }
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

        if (isGettingHit)
        {
            float _animationTimer = animationDuration;
            if (hitFedeIn)
            {
                animationWeight = Mathf.MoveTowards(animationWeight,1,Time.deltaTime * 10);
                playerController.animator.SetLayerWeight(animationHitLayer,animationWeight);
                hitAnimTimer -= Time.deltaTime;
                if (hitAnimTimer <= 0)
                {
                    hitFedeIn = false;
                    if (playerController.characterMovement.isRunning)
                    {
                        playerController.characterMovement.ToggleRunState(CharacterMovement.MoveStates.Run);
                    }
                    else
                    {
                        playerController.characterMovement.ToggleRunState(CharacterMovement.MoveStates.Walk);
                    }
                    
                }
            }
            else
            {
                animationWeight = Mathf.MoveTowards(animationWeight, 0, Time.deltaTime * 10);
                playerController.animator.SetLayerWeight(animationHitLayer, animationWeight);
                if (animationWeight == 0)
                {
                    hitDelayTimer -= Time.deltaTime;
                    if (hitDelayTimer <= 0)
                    {
                        isGettingHit = false;

                    }

                }
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
