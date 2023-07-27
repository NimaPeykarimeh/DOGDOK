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
        if (!isOpen && Input.GetKeyDown(KeyCode.T)) //Açma
        {
            isOpen = true;
            TurretPanel.SetActive(true);
        }
        else if(isOpen && Input.GetKeyDown(KeyCode.T)) //Kapama
        {
            isOpen = false;
            TurretPanel.SetActive(false);
        }
    }
}
