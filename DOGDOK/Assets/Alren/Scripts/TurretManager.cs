using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    [SerializeField] private GameObject TurretPanel;
    // Start is called before the first frame update
    void Start()
    {
        TurretPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            TurretPanel.SetActive(true);
        }
    }
}
