using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretNullControl : MonoBehaviour
{
    [HideInInspector] public bool isViable;
    private bool isColliding;
    private Renderer Renderer;
    [SerializeField] private TurretGroundedControl TurretGroundedControl;

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
    }
    private void Start()
    {
        Renderer.material.color = Color.green;
        isViable = false;
        isColliding = false;
        TurretGroundedControl.isGrounded = false; 
    }

    private void Update()
    {
        isViable = !isColliding && TurretGroundedControl.isGrounded;
        Renderer.material.color = isViable ? Color.green : Color.red;
    }
    private void OnTriggerEnter(Collider other) => isColliding = true;
    private void OnTriggerExit(Collider other) => isColliding = false;
}