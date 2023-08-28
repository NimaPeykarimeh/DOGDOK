using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour //Collect iþlemi ve kontrolünün yapýldýðý script. Collect yapýlýnca miktara ekleme yapýlýyor.
{
    private bool isCollectibleFound;
    private float timer;
    private int count;
    private int objectID;
    private Resource1 resource;
    [SerializeField] private GameObject boyutBozar;
    [SerializeField] private InventoryManager InventoryManager;
    [SerializeField] private WeaponController WeaponController;

    [Header("Attributes")]
    [SerializeField] private float collectingDistance = 5f;
    [SerializeField] private float collectingTime = 1f;
    private Ray ray;

    private void Update()
    {
        if (boyutBozar.activeInHierarchy && WeaponController.canShoot && Input.GetMouseButton(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!isCollectibleFound)
            {
                FirstCollectibleHit();
            }
            else Gathering();
        }
    }

    private void FirstCollectibleHit()
    {
        if (Physics.Raycast(ray, out RaycastHit hit, collectingDistance))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.TryGetComponent<ResourceCreation>(out ResourceCreation resourceCreation))
            {
                isCollectibleFound = true;
                resource = resourceCreation.resource;
                count = resourceCreation.resourceCount;
                objectID = hitObject.GetInstanceID();
                timer = 0;
            }
        }
    }

    private void Gathering()
    {
        if (Physics.Raycast(ray, out RaycastHit hit, collectingDistance))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.GetInstanceID() == objectID)
            {
                timer += Time.deltaTime;
                if (timer > collectingTime)
                {
                    Destroy(hitObject);
                    isCollectibleFound = false;
                    foreach (var element in InventoryManager.resourceIndices)
                    {
                        if (element.Key.id == resource.id)
                        {
                            InventoryManager.resourceIndices[element.Key] += count;
                            break;
                        }
                    }
                }
            }
            else FirstCollectibleHit();
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    timer = 0;
    //    if (other.CompareTag("Collectible"))
    //    {
    //        resource = other.gameObject.GetComponent<ResourceCreation>().resource;
    //        count = other.gameObject.GetComponent<ResourceCreation>().resourceCount;
    //    }

    //}
    //private void OnTriggerStay(Collider other)
    //{
    //    if (boyutBozar.activeInHierarchy && other.CompareTag("Collectible") && Input.GetKey(KeyCode.Mouse0))
    //    {
    //        timer += 10 * Time.deltaTime;
    //        if (timer > 5.03f)
    //        {
    //            Destroy(other.gameObject);
    //            foreach (var element in InventoryManager.resourceIndices)
    //            {
    //                if (element.Key.id == resource.id)
    //                {
    //                    InventoryManager.resourceIndices[element.Key] += count;
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        timer = 0;
    //    }
    //}
}
