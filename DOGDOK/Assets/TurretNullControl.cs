using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretNullControl : MonoBehaviour
{
    [HideInInspector] public bool isViable = true;
    private Renderer Renderer;
    // Start is called before the first frame update

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
    }
    private void Start()
    {
        Renderer.material.color = Color.green;
        isViable = true; // Ýnþa etme spamlanýrsa bug oluþabilir.
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Ground"))
        {
            print("a");
            
            isViable = false;
            Renderer.material.color = Color.red;
        }
        else
        {
            print("b");
            Renderer.material.color = Color.green;
            isViable = true;
        }
    }
}
