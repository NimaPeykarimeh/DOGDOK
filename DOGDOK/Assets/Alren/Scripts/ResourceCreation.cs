using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCreation : MonoBehaviour
{
    [SerializeField] InventoryManager InventoryManager;
    public List<Resource1> resourceTypeList = new();
    public List<int> resourceCountList = new();
    private Renderer Renderer;
    void Start()
    {
        Renderer = gameObject.GetComponent<Renderer>();
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
