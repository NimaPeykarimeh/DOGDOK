using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretNullControl : MonoBehaviour
{
    [HideInInspector] public Renderer Renderer;

    private void Awake()
    {
        Renderer = gameObject.GetComponent<Renderer>();
    }
    private void Start()
    {
        Renderer.material.SetColor("_Main_Color", Color.red);
    }
}