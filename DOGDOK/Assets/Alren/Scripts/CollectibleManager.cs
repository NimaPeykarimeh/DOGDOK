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
        objectName = other.gameObject.name; //iki farkl� trigger aras� triggerdan ��kmadan ge�i� yaparken bug ya�anmas�n diye if'te de kulland�m.
    }
    private void OnTriggerStay(Collider other)
    {
        //Sol t�k'a bas�l� tutuldu�u ve alanda durdu�u s�rece loot yapar.
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
