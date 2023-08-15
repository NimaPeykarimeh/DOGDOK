using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretNullControl : MonoBehaviour
{
    [HideInInspector] public bool isViable;
    [HideInInspector] public Renderer Renderer;

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
    }
    private void Start()
    {
        Renderer.material.color = Color.green;
        isViable = false;
    }
}