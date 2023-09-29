using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InventoryManager InventoryManager;
    [SerializeField] private TurretManager TurretManager;
    [SerializeField] private PauseManager PauseManager;
    void Update()
    {
        #region Close Everything

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (TurretManager.isOpen)
            {
                TurretManager.CloseCraftScreen();
                InventoryManager.Tablet.SetActive(false);
            }
            if (InventoryManager.isOpen)
            {
                InventoryManager.CloseInventoryMenu();
                InventoryManager.Tablet.SetActive(false);
            }
            if (InventoryManager.isBuilding)
            {
                InventoryManager.CancelBuilding();
            }
            if(!TurretManager.isOpen && !InventoryManager.isOpen && !InventoryManager.isBuilding && !PauseManager.gameIsPaused)
            {
                PauseManager.PauseGame();
            }
            else if(!TurretManager.isOpen && !InventoryManager.isOpen && !InventoryManager.isBuilding)
            {
                PauseManager.Resume();
            }
        }


        #endregion

        #region InventoryUI

        if (!InventoryManager.isBuilding && !PauseManager.gameIsPaused)
        {
            if (InventoryManager.isOpen && Input.GetKeyDown(KeyCode.E)) // Kapama
            {
                InventoryManager.CloseInventoryMenu();
                TurretManager.CloseCraftScreen();
                InventoryManager.Tablet.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.E)) // Açma
            {
                InventoryManager.OpenInventoryMenu();
                TurretManager.OpenCraftScreen();
                InventoryManager.Tablet.SetActive(true);
            }
            else if (InventoryManager.isOpen) // Açýkkene
            {
                InventoryManager.UpdateInventoryMenu();
                TurretManager.TurretPanel.GetComponent<TurretPanelCreator>().UpdateRequiredResource();
            }
        }

        #endregion

        #region TurretBuilding

        if (!PauseManager.gameIsPaused && InventoryManager.isBuilding && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.I)))
        {
            InventoryManager.CancelBuilding();
        }

        #endregion

    }
}
