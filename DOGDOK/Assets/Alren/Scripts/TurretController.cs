using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] Build1 build;
    [Header("Generating")]
    [SerializeField] Material mainMaterial;
    public Material holoMat;
    [SerializeField] const float generatedValue = -0.1f;
    [SerializeField] float dissolveValue;
    [SerializeField] const float nonGeneratedValue = 1;
    [SerializeField] bool isGenerating = false;
    public bool readyToUse = false;
    public bool itHasEnergy;
    public LayerMask _layer;

    void Start()
    {
        
        
    }


    public void StartGenerating()
    {
        transform.gameObject.layer = 7;
        isGenerating = true;
        dissolveValue = nonGeneratedValue;
        foreach (Transform _child in transform)
        {
            if(_child.TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
                meshRenderer.material = mainMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartGenerating();
        }
        if (isGenerating)
        {
            dissolveValue = Mathf.MoveTowards(dissolveValue, generatedValue, (1 / build.buildingDuration) * Time.deltaTime);
            foreach (Transform _child in transform)
            {
                if(_child.TryGetComponent<Renderer>(out Renderer renderer))
                {
                    renderer.material.SetFloat("_Dissolve", dissolveValue);
                }
            }
            //mainMaterial.SetFloat("_Dissolve", dissolveValue);
            if (dissolveValue == generatedValue)
            {
                isGenerating = false;
                readyToUse = true;
            }
        }
    }
}
