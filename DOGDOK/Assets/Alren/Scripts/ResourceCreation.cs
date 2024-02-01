using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCreation : MonoBehaviour
{
    //public List<Resource1> resourceTypeList = new();
    //public List<int> resourceCountList = new();
    private Renderer Renderer;
    public List<ResourceClass> resource;
    public float regenSec = 300;
    public float dissolvingDuration = 1;
    public float generatingDuration = 0.5f;

    [System.Serializable]
    public class ResourceClass
    {
        public Resource1 resourceType;
        public int resourceCount;
    }

    void Start()
    {
        Renderer = gameObject.GetComponent<Renderer>();
        gameObject.layer = 15;
    }

    public float DissolveCollectible(float currentAnimationValue, float dissolvedValue)
    {
        currentAnimationValue = Mathf.MoveTowards(currentAnimationValue, dissolvedValue, (1 / dissolvingDuration) * Time.deltaTime);
        Renderer.material.SetFloat("_Dissolve", currentAnimationValue);
        return currentAnimationValue;
    }

    public float GenerateCollectible(float currentAnimationValue, float unsolvedValue)
    {
        currentAnimationValue = Mathf.MoveTowards(currentAnimationValue, unsolvedValue, (1 / generatingDuration) * Time.deltaTime);
        Renderer.material.SetFloat("_Dissolve", currentAnimationValue);
        return currentAnimationValue;
    }
}
