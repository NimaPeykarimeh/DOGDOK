using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCreation : MonoBehaviour
{
    [SerializeField] InventoryManager InventoryManager;
    public Resource1 resource;
    [SerializeField] private bool isRandomized = false;
    void Start()
    {
        if (isRandomized)
        {
            int index = Random.Range(0, InventoryManager.resourceIndices.Count);
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
}
