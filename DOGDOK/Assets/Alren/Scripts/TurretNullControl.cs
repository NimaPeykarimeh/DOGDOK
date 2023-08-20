using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretNullControl : MonoBehaviour
{
    [HideInInspector] public Renderer Renderer;
    private List <Renderer> smallChildsRenderer = new();

    private void Awake()
    {
        Renderer = gameObject.GetComponent<Renderer>();
    }
    private void Start()
    {
        Material sharedMaterial = Renderer.sharedMaterial;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            for (int j = 0; j < child.childCount; j++)
            {
                Transform smallChild = child.GetChild(j);
                Collider[] colliders = smallChild.GetComponents<Collider>();
                smallChildsRenderer.Add(smallChild.GetComponent<Renderer>());
                smallChildsRenderer[j].material = sharedMaterial;
                foreach (Collider collider in colliders)
                {
                    collider.isTrigger = true;
                }
            }
        }
        Renderer.material.SetColor("_Main_Color", Color.red);
    }

    public void TurretColorSelector(bool viability)
    {
        if (viability)
        {
            for (int i = 0; i < smallChildsRenderer.Count; i++)
            {
                smallChildsRenderer[i].material.SetColor("_Main_Color", Color.green);
            }
            
            return;
        }
        for (int i = 0; i < smallChildsRenderer.Count; i++)
        {
            smallChildsRenderer[i].material.SetColor("_Main_Color", Color.red);
        }
        
    }
}