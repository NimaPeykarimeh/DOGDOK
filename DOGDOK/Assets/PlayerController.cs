using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterMovement characterMovement;
    public PlayerMouseLook playerMouseLook;
    public Animator animator;
    public bool isAiming;
    public bool isShooting;
    public bool isMoving;
    public bool isRunning;
    public bool isWalking;

    void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        playerMouseLook = GetComponent<PlayerMouseLook>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("MovementSpeed", characterMovement.currentSpeed);
        
        if (isAiming)
        {
            animator.SetLayerWeight(1,1);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }
}
