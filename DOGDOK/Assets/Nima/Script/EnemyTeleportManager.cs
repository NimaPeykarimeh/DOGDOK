using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTeleportManager : MonoBehaviour
{
    EnemyController enemyController;
    EnemyMovement enemyMovement;

    bool wasItAlerted = false;
    public bool isTeleporting;
    bool isDissolving;
    bool isGenerating;
    Vector3 positionToTeleport;

    [Header("Disolving time")]
    [SerializeField] float dissolveStart = -0.2f;
    [SerializeField] float dissolveEnd = 1f;
    [SerializeField] float dissolveDuration;
    [SerializeField] float dissolveTimer;

    private void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyController = GetComponent<EnemyController>();
    }

    public void Teleport(Vector3 _positon)
    {
        wasItAlerted = enemyController.isAlerted;
        enemyController.AlertEnemy(false,false,false,transform);
        enemyMovement.SwitchMovmentState(EnemyMovement.MovementState.Idle);
        //enemyMovement.canMove = false;
        isDissolving = true;
        isTeleporting = true;
        isGenerating = false;
        positionToTeleport= new Vector3(_positon.x,transform.position.y,_positon.z);
        enemyController.material.SetColor("_EdgeColor", enemyController.material.GetColor("_TeleportColor")) ;
    }
    private void Update()
    {
        if (isDissolving)
        {
            dissolveTimer += Time.deltaTime;
            float _dissolveRatio = Mathf.Lerp(dissolveStart, dissolveEnd, dissolveTimer / dissolveDuration); 
            if (_dissolveRatio >= dissolveEnd)
            {
                transform.position = positionToTeleport;
                isDissolving = false;
                isGenerating = true;
                _dissolveRatio = dissolveEnd;
            }
            enemyController.material.SetFloat("_Dissolve", _dissolveRatio);
        }

        if (isGenerating)
        {
            dissolveTimer -= Time.deltaTime;
            float _dissolveRatio = Mathf.Lerp(dissolveStart, dissolveEnd, dissolveTimer / dissolveDuration);
            if (_dissolveRatio <= dissolveStart)
            {
                isGenerating = false;
                isTeleporting = false;
                enemyMovement.SwitchMovmentState(enemyMovement.previousState);
                //enemyMovement.canMove = true;
                if (wasItAlerted)
                {
                    enemyController.AlertEnemy(true,false,true,transform);
                }
                _dissolveRatio = dissolveStart;
                
            }
            enemyController.material.SetFloat("_Dissolve", _dissolveRatio);
        }
    }
}
