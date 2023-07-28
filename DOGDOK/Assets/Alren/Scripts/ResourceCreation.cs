using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCreation : MonoBehaviour
{
    [SerializeField] CollectibleManager CollectibleManager;
    [HideInInspector] public int id;
    // Start is called before the first frame update
    void Start()
    {
        int index = Random.Range(0,CollectibleManager.resourceIndices.Count);
        gameObject.GetComponent <MeshFilter>().mesh = CollectibleManager.resourceIndices[index].resourceMesh;
        id = CollectibleManager.resourceIndices[index].id;
        print(index);
    }
}
