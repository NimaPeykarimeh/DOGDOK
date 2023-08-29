using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCreation : MonoBehaviour
{
    [SerializeField] InventoryManager InventoryManager;
    public Resource1 resource;
    public int resourceCount = 1;
    [SerializeField] private bool isRandomized = false;
    private Renderer Renderer;
    void Start()
    {
        Renderer = gameObject.GetComponent<Renderer>();
        if (isRandomized)
        {
            int index = Random.Range(0, InventoryManager.resourceIndices.Count);
            resourceCount = Random.Range(1, 10);
            foreach (var element in InventoryManager.resourceIndices.Keys)
            {
                if (element.id == index)
                {
                    resource = element;
                    gameObject.GetComponent<MeshFilter>().mesh = element.resourceMesh;
                    return;
                }
            }
        }
        gameObject.GetComponent<MeshFilter>().mesh = resource.resourceMesh;
    }

    public float DissolveCollectible(float currentAnimationValue, float dissolvedValue, float dissolvingDuration)
    {
        currentAnimationValue = Mathf.MoveTowards(currentAnimationValue, dissolvedValue, (1 / dissolvingDuration) * Time.deltaTime);
        Renderer.material.SetFloat("_Dissolve", currentAnimationValue);
        return currentAnimationValue;
    }

    public float GenerateCollectible(float currentAnimationValue, float unsolvedValue, float generatingDuration)
    {
        currentAnimationValue = Mathf.MoveTowards(currentAnimationValue, unsolvedValue, (1 / generatingDuration) * Time.deltaTime);
        Renderer.material.SetFloat("_Dissolve", currentAnimationValue);
        return currentAnimationValue;
    }
}
