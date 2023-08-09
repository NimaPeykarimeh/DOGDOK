using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    private bool isInventoryOpen;
    private Animator animator;
    private List<TextMeshProUGUI> UIAmount = new();
    private Build1 currentBuild;
    private Transform cubeLocation;
    private TurretNullControl TurretNullControl;

    [SerializeField] private GridDisplay GridDisplay;
    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private GameObject resBlockPrefab;
    [SerializeField] private List<Resource1> resources = new();

    [HideInInspector] public Dictionary<Resource1, int> resourceIndices = new();

    private void Awake() //resource id'leri otomatik atanýyor.
    {
        int i = 0;
        foreach (var res in resources)
        {
            res.id = i;
            resourceIndices.Add(res, 0);
            i++;
        }
    }
    void Start()
    {
        CreateInventoryMenu();
        animator = InventoryPanel.GetComponent<Animator>();
        isInventoryOpen = false;
    }

    void Update()
    {
        if (isInventoryOpen && Input.GetKeyDown(KeyCode.I)) // Envanter Açýkken Kapama
        {
            animator.SetBool("isShowing", false);
            isInventoryOpen = false;
        }
        else if (Input.GetKeyDown(KeyCode.I)) // Envanter Kapalýyken Açma
        {
            UpdateInventoryMenu();
            animator.SetBool("isShowing", true);
            isInventoryOpen = true;
        }
        else if (isInventoryOpen) // Envanter Açýk Duruyorsa.
        {
            UpdateInventoryMenu();
        }

        if (currentBuild != null) // Turret'ý yarat ve location'ýný al.
        {
            cubeLocation = CreateTurret();
        }
        else if (cubeLocation != null) // Mouse'u takip ederek Turret'ýn koyulacaðý yere bak ve güncelle.
        {
            TrackMouseForBuilding();
        }
    }

    private Transform CreateTurret()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 positionToPlace = hit.point;

            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                positionToPlace = PlaceObjectOnGrid(positionToPlace, currentBuild.buildingSize);
                turretPrefab.transform.localScale = GridDisplay.cellSize * currentBuild.buildingSize; //scale'i !!!!!!
                positionToPlace.y += turretPrefab.transform.localScale.y / 2; //küp yüksekliði
                Transform t = Instantiate(turretPrefab, positionToPlace, transform.rotation).GetComponent<Transform>();
                TurretNullControl = t.GetComponent<TurretNullControl>();
                currentBuild = null;
                return t;
            }
        }
        return null;

        // Zombieleri takip eden silah namlusu
        // NULL kontrolü yap.
        // Player'dan uzaksa yapamasýn.
    }
    private Vector3 PlaceObjectOnGrid(Vector3 position, Vector3 size)
    {
        float x = Mathf.RoundToInt(position.x / GridDisplay.cellSize);
        x = x * GridDisplay.cellSize - GridDisplay.cellSize / 2;
        if (size.x % 2 == 0)
        {
            x -= GridDisplay.cellSize / 2;
        }

        float y = Mathf.RoundToInt(position.y / GridDisplay.cellSize);

        float z = Mathf.RoundToInt(position.z / GridDisplay.cellSize);
        z = z * GridDisplay.cellSize - GridDisplay.cellSize / 2;
        if (size.z % 2 == 0)
        {
            z -= GridDisplay.cellSize / 2;
        }

        Vector3 snappedPosition = new Vector3(x, y * GridDisplay.cellSize + GridDisplay.cellSize/2, z);
        return snappedPosition;
    }

    private void TrackMouseForBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 positionToPlace = hit.point;
            cubeLocation.position = PlaceObjectOnGrid(positionToPlace, cubeLocation.localScale);
            if (Input.GetMouseButtonDown(0) && TurretNullControl.isViable)
            {
                cubeLocation.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                cubeLocation = null;
            }
        }
    }

    private void CreateInventoryMenu()
    {
        int i = 0;
        foreach (var element in resources)
        {
            GameObject resBlock = Instantiate(resBlockPrefab, InventoryPanel.transform);
            resBlock.transform.Find("resText").GetComponent<TextMeshProUGUI>().text = element.resourceName;
            UIAmount.Add(resBlock.transform.Find("resAmountText").GetComponent<TextMeshProUGUI>());
            UIAmount[i].text = "0";
            i++;
        }
    }

    private void UpdateInventoryMenu()
    {
        int i = 0;
        foreach (var element in resourceIndices.Values)
        {
            UIAmount[i].text = element.ToString();
            i++;
        }
    }
    public bool UseResources(Dictionary<Resource1, int> neededResources) //kaynaðýn türünü sayý cinsinden ve miktarýný giriniz.
    {
        //Kaynak yeterliliði kontrolü
        foreach (var need in neededResources)
        {
            foreach (var have in resourceIndices)
            {
                if (have.Key == need.Key)
                {
                    if (have.Value - need.Value < 0)
                    {
                        return false;
                    }
                }
            }
        }
        Dictionary<Resource1, int> tempHave = resourceIndices.ToDictionary(entry => entry.Key, entry => entry.Value);
        //Deðeri Güncelle
        foreach (var need in neededResources)
        {
            foreach (var have in tempHave)
            {
                if (have.Key == need.Key)
                {
                    resourceIndices[have.Key] -= need.Value;
                }
            }
        }
        return true;
    }
    public void SetCurrentBuild(Build1 build) => currentBuild = build;
}