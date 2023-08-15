using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    public GameObject TurretPanel;
    [HideInInspector] public bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        TurretPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
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
