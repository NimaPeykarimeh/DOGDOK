using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyPartDamageDetection : MonoBehaviour
{
    [SerializeField] EnemyHealth enemyHealth;
    [SerializeField] byte headShotMult = 1;
    [SerializeField] BodyParts bodyPart;
    [SerializeField] int maxPartDamageLimit = 25;
    [SerializeField] int currentPartDamage = 25;
    [SerializeField] bool isPartOff;
    [SerializeField] bool isTearable = false;
    [Header("FallingPart")]
    [SerializeField] GameObject fallingPart;
    [SerializeField] Material fallingPartMat;
    [SerializeField] float dissolveDuration = 0.5f;
    [SerializeField] bool isDissolving = false;
    [SerializeField] float dissolveValue = 0;
    [ColorUsage(true, true)]
    [SerializeField] Color partDissolveColor;
    [SerializeField] bool isFalling;
    public enum BodyParts
    {
        Head,
        rightArm,
        leftArm,
        leg,
        torso
    }

    private void Start()
    {
        ResetPartValues();

        if (isTearable)
        {
            fallingPartMat = fallingPart.GetComponent<Renderer>().material;
            fallingPart.SetActive(false);
        }
    }
    public void GetPartDamage(int _damage, EnemyHealth.HitSource _hitSource)//move headshot to enemyhealth
    {
        if (currentPartDamage > 0)
        {
            currentPartDamage -= _damage;
            enemyHealth.GetDamage(_damage * headShotMult, _hitSource);
        }
        else if (currentPartDamage <= 0 && !isPartOff && isTearable)
        {
            isPartOff = true;
            transform.localScale = Vector3.zero;
            if (bodyPart == BodyParts.leg)
            {
                enemyHealth.enemyController.animator.SetTrigger("LegOff");
                enemyHealth.enemyController.enemyMovement.SwitchMovmentState(EnemyMovement.MovementState.Crawl);
            }
            if (bodyPart == BodyParts.rightArm)
            {
                enemyHealth.itHasRightHand = false;
            }
            if (bodyPart == BodyParts.leftArm)
            {
                enemyHealth.itHasLeftHand = false;
            }
            //falling part
            fallingPart.SetActive(true);
            if (isFalling) fallingPart.transform.parent = null;

            fallingPartMat.SetColor("_EdgeColor",partDissolveColor);
            isDissolving = true;
            dissolveValue = 0;
        }
    }

    private void Update()
    {
        if (isDissolving)
        {
            fallingPartMat.SetFloat("_Dissolve",dissolveValue);
            dissolveValue = Mathf.MoveTowards(dissolveValue, 1, (1 / dissolveDuration) * Time.deltaTime);
            if (dissolveValue == 1)
            {
                isDissolving = false;
                if (isFalling)
                {
                    fallingPart.transform.parent = null;
                    fallingPart.transform.localScale = Vector3.one;
                }
                fallingPart.SetActive(false);
            }
        }
    }

    public void ResetPartValues()
    {
        currentPartDamage = maxPartDamageLimit;
        isPartOff = false;
        transform.localScale = Vector3.one;
    }
}
