using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCreation : MonoBehaviour
{
    [SerializeField] CollectibleManager CollectibleManager;
    //[HideInInspector] public int id;
    public Resource1 resource;
    [SerializeField] private bool isRandomized = false;
    // Start is called before the first frame update
    void Start()
    {
        //if (isRandomized)
        //{
        //    int index = Random.Range(0, CollectibleManager.resourceIndices.Count);
        //    gameObject.GetComponent<MeshFilter>().mesh = CollectibleManager.resourceIndices[index].resourceMesh;
        //    id = CollectibleManager.resourceIndices[index].id;
        //    //print(index);
        //    return;
        //}
        if (isRandomized)
        {
            int index = Random.Range(0, CollectibleManager.resourceIndices.Count);
            foreach(var element in CollectibleManager.resourceIndices.Keys)
            {
                if(element.id == index)
                {
                    resource = element;
                    gameObject.GetComponent<MeshFilter>().mesh = element.resourceMesh;
                    return;
                }
            }
        }
        gameObject.GetComponent<MeshFilter>().mesh = resource.resourceMesh;
    }
}
