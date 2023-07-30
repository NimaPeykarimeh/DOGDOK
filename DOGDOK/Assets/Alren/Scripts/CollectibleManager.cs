using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    private float timer;
    private Resource1 resource;
    [SerializeField] private InventoryManager InventoryManager;
    [HideInInspector] public Dictionary<Resource1, int> resourceIndices = new Dictionary<Resource1, int>();
    [SerializeField] private List<Resource1> resources = new();

    private void Awake()
    {
        foreach(var res in resources)
        {
            resourceIndices.Add(res, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        timer = 0;
        if (other.CompareTag("Collectible"))
        {
            resource = other.gameObject.GetComponent<ResourceCreation>().resource;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Collectible") && Input.GetKey(KeyCode.Mouse0))
        {
            timer += 10 * Time.deltaTime;
            if (timer > 5.03f)
            {
                Destroy(other.gameObject);
                foreach (var element in resourceIndices)
                {
                    if (element.Key == resource)
                    {
                        resourceIndices[element.Key]++;
                        break;
                    }
                }
            }
        }
        else
        {
            timer = 0;
        }
    }
}
