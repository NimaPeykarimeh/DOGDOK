using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InventoryManager InventoryManager;
    [SerializeField] private TurretManager TurretManager;
    void Update()
    {
        #region Close Everything

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (TurretManager.isOpen)
            {
                TurretManager.CloseCraftScreen();
            }
            if (InventoryManager.isOpen)
            {
                InventoryManager.CloseInventoryMenu();
            }
            if (InventoryManager.isBuilding)
            {
                InventoryManager.CancelBuilding();
            }
        }


        #endregion

        #region InventoryUI

        if (!TurretManager.isOpen && !InventoryManager.isBuilding)
        {
            if (InventoryManager.isOpen && Input.GetKeyDown(KeyCode.I)) // Kapama
            {
                InventoryManager.CloseInventoryMenu();
            }
            else if (Input.GetKeyDown(KeyCode.I)) // Açma
            {
                InventoryManager.OpenInventoryMenu();
            }
            else if (InventoryManager.isOpen) // Açýkkene
            {
                InventoryManager.UpdateInventoryMenu();
            }
        }

        #endregion

        #region TurretSelectionUI

        if (!InventoryManager.isOpen && !InventoryManager.isBuilding)
        {
            if (!TurretManager.isOpen && Input.GetKeyDown(KeyCode.E)) //Açma
            {
                TurretManager.OpenCraftScreen();
            }
            else if (Input.GetKeyDown(KeyCode.E)) //Kapama
            {
                TurretManager.CloseCraftScreen();
            }
            else if (TurretManager.isOpen)
            {
                TurretManager.TurretPanel.GetComponent<TurretPanelCreator>().UpdateRequiredResource();
            }
        }

        #endregion

        #region TurretBuilding
        if (InventoryManager.isBuilding && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.I)))
        {
            InventoryManager.CancelBuilding();
        }
        

        #endregion

    }
}
