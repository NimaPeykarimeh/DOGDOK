using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTeleportManager : MonoBehaviour
{
    EnemyController enemyController;
    EnemyMovement enemyMovement;
    [SerializeField] SkinnedMeshRenderer mesh;
    [SerializeField] Material material;

    public bool isTeleporting;
    bool isDissolving;
    bool isGenerating;
    Vector3 positionToTeleport;

    [Header("Disolving time")]
    [SerializeField] float dissolveStart = -0.2f;
    [SerializeField] float dissolveEnd = 1f;
    [SerializeField] float dissolveDuration;
    [SerializeField] float dissolveTimer;

    private void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyController = GetComponent<EnemyController>();
        material = mesh.material;
        
    }
    public void Teleport(Vector3 _positon)
    {
        enemyController.isAlerted = false;
        enemyMovement.canMove = false;
        isDissolving = true;
        isTeleporting = true;
        isGenerating = false;
        positionToTeleport= new Vector3(_positon.x,transform.position.y,_positon.z);
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
            material.SetFloat("_Dissolve", _dissolveRatio);
        }

        if (isGenerating)
        {
            dissolveTimer -= Time.deltaTime;
            float _dissolveRatio = Mathf.Lerp(dissolveStart, dissolveEnd, dissolveTimer / dissolveDuration);
            if (_dissolveRatio <= dissolveStart)
            {
                isGenerating = false;
                isTeleporting = false;
                enemyMovement.canMove = true;
                enemyController.isAlerted = true;
                _dissolveRatio = dissolveStart;
                
            }
            material.SetFloat("_Dissolve", _dissolveRatio);
        }
    }
}
