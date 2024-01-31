using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TurretNullControl))]
public class TurretController : MonoBehaviour
{
    public Build1 build;
    [Header("Generating")]
    public List<Material> mainMaterials;
    public Material holoMat;
    [SerializeField] const float generatedValue = -0.1f;
    [SerializeField] float dissolveValue;
    [SerializeField] const float nonGeneratedValue = 1;
    [SerializeField] bool isGenerating = false;
    public bool readyToUse = false;
    public bool itHasEnergy;
    public LayerMask _layer;
    [SerializeField] int turretLayerIndex = 9;
    private TurretCollect TurretCollect;
    [SerializeField] bool isBuilded = true;

    private void Start()
    {
        if (isBuilded)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
                {
                    meshRenderer.material = holoMat;
                }
            }

        }
        TurretCollect = GetComponent<TurretCollect>();
    }

    public void StartGenerating()
    {
        transform.gameObject.layer = turretLayerIndex;
        isGenerating = true;
        dissolveValue = nonGeneratedValue;
        
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
            {
                meshRenderer.material = mainMaterials[i];
            }
            if (transform.GetChild(i).TryGetComponent<Collider>(out Collider _collider))
            {
                _collider.enabled = true;
            }
        }
        //foreach (Transform _child in transform)
        //{
        //    if(_child.TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
        //        meshRenderer.material = mainMaterials[i];
        //}
    }

    // Update is called once per frame
    void Update()
    {
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
                TurretCollect.GatherRenderer();
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartGenerating();
        }
    }
}
