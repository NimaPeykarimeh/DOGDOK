using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    private float timer;
    private string objectName;
    [SerializeField] private InventoryManager InventoryManager;
    public List<Resource1> resourceIndices = new List<Resource1>();

    private void Start()
    {
        for (int i = 0; i < resourceIndices.Count; i++)
        {
            print(resourceIndices[i].resourceName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        timer = 0;
        objectName = other.gameObject.name;
    }
    private void OnTriggerStay(Collider other)
    {
        print(other.gameObject.name);
        print(objectName);
        //Sol týk'a basýlý tutulduðu ve alanda durduðu sürece loot yapar.
        if (other.CompareTag("Collectible") && Input.GetKey(KeyCode.Mouse0))
        {
            timer += 10 * Time.deltaTime;
            if (timer > 5)
            {
                Destroy(other.gameObject);
                for (int i = 0; i < resourceIndices.Count; i++)
                {
                    if (resourceIndices[i].resourceName == objectName)
                    {
                        InventoryManager.InventorySlots[i]++;
                        break;
                    }
                }                
                //if (resourceIndices.TryGetValue(objectName, out int resourceType))
                //{
                //    InventoryManager.InventorySlots[resourceType]++;
                //}
            }
        }
        else
        {
            timer = 0;
        }
    }
}
