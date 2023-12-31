using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class EnemyHealth : MonoBehaviour
{
    public EnemyController enemyController;
    [SerializeField] int headShotMult;
    [SerializeField] int maxHealth = 100;
    public int currentHealth;
    public Rigidbody[] bodyPartRb;
    [SerializeField] float dieForce = 1;
    [Header("Dead Animation")]
    float dissolveValue = 0;
    public bool isDying = false;
    [SerializeField] float animationDuration = 1f;
    Vector3[] bodyPartPositions;
    Quaternion[] bodyPartRotation;
    [ColorUsage(true, true)]
    [SerializeField] Color deadByPlayerColor;
    [ColorUsage(true, true)]
    [SerializeField] Color deadByRegularTurret;
    [Header("BodyPartInfo")]
    public bool itHasLeftHand = true;
    public bool itHasRightHand = true;
    public enum HitSource
    {
        Player,
        RegularTurret
    }

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    void Start()
    {
        bodyPartPositions = new Vector3[bodyPartRb.Length];
        bodyPartRotation = new Quaternion[bodyPartRb.Length];
        currentHealth = maxHealth;
        itHasLeftHand = true;
        itHasRightHand = true;
        for (int i = 0; i < bodyPartRb.Length; i++)
        {
            bodyPartRb[i].isKinematic = true;
            bodyPartPositions[i] = bodyPartRb[i].transform.localPosition;
            bodyPartRotation[i] = bodyPartRb[i].transform.localRotation;
        }

    }

    public void SetSpawnValues()
    {
        isDying = false;
        enemyController.enemyCollider.enabled = true;

        for (int i = 0; i < bodyPartRb.Length; i++)
        {
            bodyPartRb[i].velocity = Vector3.zero;
            bodyPartRb[i].isKinematic = false;
            bodyPartRb[i].transform.localPosition = bodyPartPositions[i];
            bodyPartRb[i].transform.localRotation = bodyPartRotation[i];
            bodyPartRb[i].gameObject.GetComponent<EnemyBodyPartDamageDetection>().ResetPartValues();
        }

        foreach (Rigidbody _rb in bodyPartRb)
        {
            _rb.velocity = Vector3.zero;
            _rb.isKinematic = true;
        }
        currentHealth = maxHealth;// change later
        enemyController.enemyMovement.ResetMovementState();
        enemyController.enemyFollow.enabled = true;
        enemyController.enemyMovement.enabled = true;
        enemyController.animator.enabled = true;
        enemyController.enabled = true;
        dissolveValue = 0;
        enemyController.material.SetFloat("_Dissolve", dissolveValue);
        enemyController.AlertEnemy(false,false,false,enemyController.player);
        enemyController.enemySpawner.BackToPooler(transform);
        //enemyController.enemyMovement.enemyRb.isKinematic = true;
    }

    private void Dead(HitSource _deathSource)
    {
        //enemyController.enemySpawner.BackToPooler(transform);
        //enemyController.animator.SetFloat("DeathRandomizer", Random.Range(0f,1f));
        //enemyController.animator.SetTrigger("Dead");
        //enemyController.enemyMovement.enemyRb.AddForce(transform.forward * (-dieForce),ForceMode.Impulse);
        if (!isDying)
        {
            isDying = true;
            ScoreManager.Instance.killCounter++;
            enemyController.enemyCollider.enabled = false;

            if (_deathSource == HitSource.Player)
            {
                enemyController.material.SetColor("_EdgeColor", deadByPlayerColor);
            }
            else if (_deathSource == HitSource.RegularTurret)
            {
                enemyController.material.SetColor("_EdgeColor", deadByRegularTurret);
            }
            foreach (Rigidbody _rb in bodyPartRb)
            {

                _rb.isKinematic = false;
                //_rb.GetComponent<Collider>().isTrigger = false;
                if (_rb.gameObject.name == "mixamorig:Head")
                {
                    _rb.AddForce(-transform.forward * dieForce, ForceMode.Impulse);
                }
            }
            enemyController.enemyFollow.enabled = false;
            enemyController.enemyMovement.enabled = false;
            enemyController.animator.enabled = false;
            enemyController.isTargetedTurret = false;
            //enemyController.enemyMovement.enemyRb.isKinematic = true;
            enemyController.enabled = false;
        }
        
    }

    private void Update()
    {
        if (isDying)
        {
            dissolveValue = Mathf.MoveTowards(dissolveValue, 1, (1 / animationDuration) * Time.deltaTime);
            enemyController.material.SetFloat("_Dissolve", dissolveValue);
            if (dissolveValue >= 1)
            {
                SetSpawnValues();
            }
        }
    }

    public void GetDamage(int _damage, HitSource _damageSource,Transform _hitTransform)
    {
        enemyController.AlertEnemy(true,false, _damageSource == HitSource.RegularTurret, _hitTransform);
        currentHealth -= _damage;
        if (currentHealth <= 0)
        {
            Dead(_damageSource);
        }
    }
}
