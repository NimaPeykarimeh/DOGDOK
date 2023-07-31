using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTeleportManager : MonoBehaviour
{
    EnemyController enemyController;
    [SerializeField] SkinnedMeshRenderer mesh;
    [SerializeField] Material material;

    [SerializeField] float dissolveDuration;
    [SerializeField] float dissolveTimer;

    public bool isTeleporting;
    bool isDissolving;
    bool isGenerating;
    Vector3 positionToTeleport;
    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        material = mesh.material;
        
    }
    public void Teleport(Vector3 _positon)
    {
        enemyController.isAlerted = false;
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
            float _dissolveRatio = dissolveTimer/dissolveDuration;
            if (_dissolveRatio >= 1)
            {
                transform.position = positionToTeleport;
                isDissolving = false;
                isGenerating = true;
                _dissolveRatio = 1;
            }
            material.SetFloat("_Dissolve", _dissolveRatio);
        }

        if (isGenerating)
        {
            dissolveTimer -= Time.deltaTime;
            float _dissolveRatio = dissolveTimer / dissolveDuration;
            if (_dissolveRatio <= 0)
            {
                isGenerating = false;
                isTeleporting = false;
                enemyController.isAlerted = true;
                _dissolveRatio = 0;
                
            }
            material.SetFloat("_Dissolve", _dissolveRatio);
        }
    }
}
