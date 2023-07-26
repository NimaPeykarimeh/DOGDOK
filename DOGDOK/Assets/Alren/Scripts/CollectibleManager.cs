using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    private float timer;
    private string objectName;
    [SerializeField] private InventoryManager InventoryManager;
    public Dictionary<string, int> resourceIndices;

    private void Start()
    {
        resourceIndices = new Dictionary<string, int>
        {
            { "Stone", 0 },
            { "Plank", 1 },
            { "Plate", 2 },
            { "Circuit", 3 },
            { "MetalComposite", 4 },
            { "Energy", 5 }
        };
    }

    private void OnTriggerEnter(Collider other)
    {

        timer = 0;
        objectName = other.gameObject.name; //iki farklý trigger arasý triggerdan çýkmadan geçiþ yaparken bug yaþanmasýn diye if'te de kullandým.
    }
    private void OnTriggerStay(Collider other)
    {
        //Sol týk'a basýlý tutulduðu ve alanda durduðu sürece loot yapar.
        if (other.CompareTag("Collectible") && Input.GetMouseButton(0) && objectName == other.gameObject.name)
        {
            timer += Time.deltaTime;
            if (timer > 0.501f)
            {
                Destroy(other.gameObject);
                if (resourceIndices.TryGetValue(objectName, out int resourceType))
                {
                    InventoryManager.InventorySlots[resourceType]++;
                }
            }
        }
        else
        {
            timer = 0;
            objectName = other.gameObject.name;
        }
    }
}
