using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCreation : MonoBehaviour
{
    [SerializeField] CollectibleManager CollectibleManager;
    [HideInInspector] public int id;
    [SerializeField] private bool isRandomized = false;
    // Start is called before the first frame update
    void Start()
    {
        if (isRandomized)
        {
            int index = Random.Range(0, CollectibleManager.resourceIndices.Count);
            gameObject.GetComponent<MeshFilter>().mesh = CollectibleManager.resourceIndices[index].resourceMesh;
            id = CollectibleManager.resourceIndices[index].id;
            //print(index);
            return;
        }
        id = gameObject.name switch
        {
            "Stone" => 0,
            "Plank" => 1,
            "Plate" => 2,
            "Circuit" => 3,
            "MetalComposite" => 4,
            "Energy" => 5,
            _ => -1
        };
    }
}
