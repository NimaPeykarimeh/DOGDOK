using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    [SerializeField] private GameObject TurretPanel;
    private bool isOpen;
    private Build1 currentBuild;
    // Start is called before the first frame update
    void Start()
    {
        currentBuild = null;
        isOpen = false;
        TurretPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen && Input.GetKeyDown(KeyCode.E)) //Açma
        {
            isOpen = true;
            TurretPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            TurretPanel.GetComponent<TurretPanelCreator>().UpdateRequiredResource();
        }
        else if (isOpen && Input.GetKeyDown(KeyCode.E)) //Kapama
        {
            isOpen = false;
            TurretPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
