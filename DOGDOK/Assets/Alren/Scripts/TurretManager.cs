using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    [SerializeField] private GameObject TurretPanel;
    private bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        TurretPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen && Input.GetKeyDown(KeyCode.E)) //Açma
        {
            OpenCraftScreen();
        }
        else if (isOpen && Input.GetKeyDown(KeyCode.E)) //Kapama
        {
            CloseCraftScreen();
        }
        else if(isOpen)
        {
            TurretPanel.GetComponent<TurretPanelCreator>().UpdateRequiredResource();
        }
    }
    public void OpenCraftScreen()
    {
        isOpen = true;
        TurretPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void CloseCraftScreen()
    {
        isOpen = false;
        TurretPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
