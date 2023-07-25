using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    private float timer;
    private string objectName;
    [SerializeField] private InventoryManager InventoryManager;

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
            //print(timer);
            if (timer > 0.501f)
            {
                Destroy(other.gameObject);
                if (objectName == "Stone")
                {
                    InventoryManager.InventorySlots[0]++;
                }
                else if (objectName == "Plank")
                {
                    InventoryManager.InventorySlots[1]++;
                }
                else if (objectName == "Plate")
                {
                    InventoryManager.InventorySlots[2]++;
                }
                else if (objectName == "Circuit")
                {
                    InventoryManager.InventorySlots[3]++;
                }
                else if (objectName == "MetalComposite")
                {
                    InventoryManager.InventorySlots[4]++;
                }
                else if (objectName == "Energy")
                {
                    InventoryManager.InventorySlots[5]++;
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
