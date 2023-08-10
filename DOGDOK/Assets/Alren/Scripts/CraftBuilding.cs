using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftBuilding : MonoBehaviour
{
    [HideInInspector] public int id;
    [SerializeField] private InventoryManager InventoryManager;
    [SerializeField] private TurretPanelCreator TurretPanelCreator;
    [SerializeField] private TurretManager TurretManager;
    // Start is called before the first frame update

    private void Awake()
    {
        InventoryManager = GameObject.Find("UI & Manager/Inventory Manager").GetComponent<InventoryManager>();
        TurretPanelCreator = GameObject.Find("UI & Manager/Canvas/Panel/Turret Panel").GetComponent<TurretPanelCreator>();
        TurretManager = GameObject.Find("UI & Manager/Turret Manager").GetComponent<TurretManager>();
    }

    public void UseButtonClick()
    {
        Dictionary<Resource1, int> needs = new();
        for (int i = 0; i < TurretPanelCreator.builds[id].requiredResource.Count; i++)
        {
            needs.Add(TurretPanelCreator.builds[id].requiredResource[i], TurretPanelCreator.builds[id].requiredAmount[i]);
        }

        if (InventoryManager.CheckResources(needs))
        {
            print("it can");
            TurretManager.CloseCraftScreen();
            InventoryManager.SetCurrentBuild(TurretPanelCreator.builds[id]);
        }
        else print("it cant");
    }
}
