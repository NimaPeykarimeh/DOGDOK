using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour //Collect iþlemi ve kontrolünün yapýldýðý script. Collect yapýlýnca miktara ekleme yapýlýyor.
{
    private float timer;
    private Resource1 resource;
    [SerializeField] private InventoryManager InventoryManager;
    

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
                foreach (var element in InventoryManager.resourceIndices)
                {
                    if (element.Key == resource)
                    {
                        InventoryManager.resourceIndices[element.Key]++;
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
