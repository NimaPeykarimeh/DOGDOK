using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftBuilding : MonoBehaviour
{
    [HideInInspector] public int id;
    [SerializeField] private InventoryManager InventoryManager;
    [SerializeField] private TurretPanelCreator TurretPanelCreator;
    // Start is called before the first frame update

    private void Awake()
    {
        InventoryManager = GameObject.Find("UI & Manager/Inventory Manager").GetComponent<InventoryManager>();
        TurretPanelCreator = GameObject.Find("UI & Manager/Canvas/Panel/Turret Panel").GetComponent<TurretPanelCreator>();
    }

    public void UseButtonClick()
    {
        Dictionary<Resource1, int> needs = new();
        //     TurretPanelCreator.builds[id].requiredResource, TurretPanelCreator.builds[id].requiredAmount
        for(int i = 0; i<  TurretPanelCreator.builds[id].requiredResource.Count; i++)
        {
            needs.Add(TurretPanelCreator.builds[id].requiredResource[i], TurretPanelCreator.builds[id].requiredAmount[i]);
        }

        if (InventoryManager.UseResources(needs))
        {
            print("it can");
        }
        else print("it cant");
    }
}
