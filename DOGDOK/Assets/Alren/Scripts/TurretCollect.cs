using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCollect : MonoBehaviour
{
    private InventoryManager inventoryManager;
    private WeaponUIManager weaponUIManager;
    [SerializeField] private List<Renderer> Renderers;
    [SerializeField] private float dissolvingDuration = 1f;
    [SerializeField] private float collectingDistance = 15f;
    private Dictionary<Resource1,int> resourceDictionary = new();
    private TurretController turretController;
    private float currentAnimationValue;
    private const float dissolvedValue = 1f;
    private const float unsolvedValue = 0f;
    private bool isDissolving;
    private Ray ray;


    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.Find("UI & Manager/Inventory Manager").GetComponent<InventoryManager>();
        weaponUIManager = GameObject.Find("UI & Manager/WeaponUI Manager").GetComponent<WeaponUIManager>();
        currentAnimationValue = unsolvedValue;
        turretController = gameObject.GetComponent<TurretController>();
        for(int i = 0; i < turretController.build.requiredResource.Count; i++)
        {
            resourceDictionary.Add(turretController.build.requiredResource[i], turretController.build.requiredAmount[i]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (turretController.readyToUse && Physics.Raycast(ray, out RaycastHit hit, collectingDistance))
        {
            if(gameObject == hit.transform.gameObject && Input.GetMouseButton(1) && weaponUIManager.weaponManager.currentWeaponIndex == 0)
            {
                print("hits object");
                for (int i = 0; i < Renderers.Count; i++)
                {
                    Renderers[i].material.SetFloat("_IsSelected", 1f);
                }
                if(Input.GetMouseButtonDown(0) && !isDissolving)
                {
                    isDissolving = true;
                }
            }
            else
            {
                for (int i = 0; i < Renderers.Count; i++)
                {
                    Renderers[i].material.SetFloat("_IsSelected", 0f);
                }
            }
        }
        else
        {
            print("no");
            for (int i = 0; i < Renderers.Count; i++)
            {
                Renderers[i].material.SetFloat("_IsSelected", 0f);
            }
        }

        if (isDissolving)
        {
            if(dissolvedValue == DissolveMaterial())
            {
                inventoryManager.AddResources(resourceDictionary);
                Destroy(gameObject);
            }
        }
    }

    public void GatherRenderer()
    {
        if (gameObject.TryGetComponent(out Renderer mainRenderer))
        {
            Renderers.Add(mainRenderer);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out Renderer renderer))
            {
                Renderers.Add(renderer);
            }
        }
    }
    private float DissolveMaterial()
    {
        currentAnimationValue = Mathf.MoveTowards(currentAnimationValue, dissolvedValue, (1 / dissolvingDuration) * Time.deltaTime);
        for(int i = 0; i < Renderers.Count; i++)
        {
            Renderers[i].material.SetFloat("_Dissolve", currentAnimationValue);
        }
        return currentAnimationValue;
    }
}
