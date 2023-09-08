using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretNullControl : MonoBehaviour
{
    [HideInInspector] public Renderer Renderer;
    
    TurretController turretController;
    private List <Renderer> smallChildsRenderer = new();

    private void Awake()
    {
        Renderer = gameObject.GetComponent<Renderer>();
    }
    private void Start()
    {
        turretController = GetComponent<TurretController>();
        turretController.holoMat.SetColor("_Main_Color", Color.yellow);
    }

    public void TurretColorSelector(bool viability)
    {
        if (viability)
        {
            turretController.holoMat.SetColor("_Main_Color", Color.green);
            return;
        }
        turretController.holoMat.SetColor("_Main_Color", Color.red);
    }
}