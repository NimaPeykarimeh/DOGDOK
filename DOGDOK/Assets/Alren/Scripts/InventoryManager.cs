using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEditor.PackageManager.Requests;

public class InventoryManager : MonoBehaviour
{
    private Animator animator;
    private List<TextMeshProUGUI> UIAmount = new();
    private Build1 currentBuild;
    private Transform cubeTransform;
    private Dictionary<Resource1, int> currentNeeds;
    private Renderer turretRenderer;

    [SerializeField] private GridDisplay GridDisplay;
    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private GameObject resBlockPrefab;
    [SerializeField] private List<Resource1> resources = new();
    [SerializeField] private byte buildDistance;

    [HideInInspector] public bool isBuilding;
    [HideInInspector] public bool isOpen;
    [HideInInspector] public Dictionary<Resource1, int> resourceIndices = new();

    private void Awake() //resource id'leri otomatik atan�yor.
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
        isOpen = false;
    }

    void Update()
    {
        if (currentBuild != null) // Turret'� yarat ve location'�n� al.
        {
            cubeTransform = CreateTurret();
        }
        else if (cubeTransform != null) // Mouse'u takip ederek Turret'�n koyulaca�� yere bak ve g�ncelle.
        {
            TrackMouseForBuilding();
        }
    }

    #region Turret Building
    public void SetCurrentBuild(Build1 build) => currentBuild = build;
    private Transform CreateTurret()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        isBuilding = true;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 positionToPlace = hit.point;

            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                positionToPlace = PlaceObjectOnGrid(positionToPlace, currentBuild.buildingSize);
                turretPrefab.transform.localScale = GridDisplay.cellSize * currentBuild.buildingSize; //scale'i !!!!!!
                positionToPlace.y += turretPrefab.transform.localScale.y / 2; //k�p y�ksekli�i
                Transform t = Instantiate(turretPrefab, positionToPlace, transform.rotation).GetComponent<Transform>();
                turretRenderer = t.GetComponent<TurretNullControl>().Renderer;
                currentBuild = null;
                return t;
            }
        }
        return null;
    }
    private Vector3 PlaceObjectOnGrid(Vector3 position, Vector3 size)
    {
        //float x = Mathf.RoundToInt(position.x / GridDisplay.cellSize);
        //x = x * GridDisplay.cellSize - GridDisplay.cellSize / 2;
        //if (size.x % 2 == 0)
        //{
        //    x += GridDisplay.cellSize / 2;
        //}

        //float y = //Mathf.RoundToInt(position.y / GridDisplay.cellSize);
        //y = 0.05f; //yere tam biti�ik olursa s�k�nt� ��k�yor.
        //float z = Mathf.RoundToInt(position.z / GridDisplay.cellSize);
        //z = z * GridDisplay.cellSize - GridDisplay.cellSize / 2;
        //if (size.z % 2 == 0)
        //{
        //    z += GridDisplay.cellSize / 2;
        //}

        //Vector3 snappedPosition = new Vector3(x, y * GridDisplay.cellSize + GridDisplay.cellSize / 2, z);

        //return snappedPosition;
        float gridSize = GridDisplay.cellSize;

        float xPosition = Mathf.Round(position.x / gridSize) * gridSize;
        float zPosition = Mathf.Round(position.z / gridSize) * gridSize;

        int xSize = Mathf.FloorToInt(size.x);
        int zSize = Mathf.FloorToInt(size.z);

        if (xSize % 2 > 0)
        {
            float xDistanceToCurrentGrid = Mathf.Abs(position.x - (xPosition - gridSize / 2));
            float xDistanceToNextGrid = Mathf.Abs(position.x - (xPosition + gridSize / 2));

            if (xDistanceToNextGrid < xDistanceToCurrentGrid)
            {
                xPosition += gridSize / 2f;
            }
            else
            {
                xPosition -= gridSize / 2f;
            }
        }

        if (zSize % 2 > 0)
        {
            float zDistanceToCurrentGrid = Mathf.Abs(position.z - (zPosition - gridSize / 2));
            float zDistanceToNextGrid = Mathf.Abs(position.z - (zPosition + gridSize / 2));

            if (zDistanceToNextGrid < zDistanceToCurrentGrid)
            {
                zPosition += gridSize / 2f;
            }
            else
            {
                zPosition -= gridSize / 2f;
            }
        }

        Vector3 snappedPosition = new Vector3(xPosition, 0.06f * GridDisplay.cellSize + GridDisplay.cellSize / 2, zPosition);
        return snappedPosition;
    }
    private void TrackMouseForBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 positionToPlace;
        if (Physics.Raycast(ray, out hit, buildDistance))
        {
            positionToPlace = hit.point;
        }
        else
        {
            positionToPlace = ray.GetPoint(buildDistance);
        }
        positionToPlace.y = 0;
        cubeTransform.position = PlaceObjectOnGrid(positionToPlace, cubeTransform.localScale);
        if (!Physics.CheckBox(cubeTransform.position, cubeTransform.localScale / 2, cubeTransform.rotation))
        {
            turretRenderer.material.SetColor("_Main_Color", Color.green);
            if (Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetKeyDown(KeyCode.E) && !Input.GetKeyDown(KeyCode.Escape))
            {

                UseResources(currentNeeds);
                cubeTransform.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                cubeTransform.gameObject.GetComponent<TurretNullControl>().enabled = false;
                isBuilding = false;
                cubeTransform = null;
                return;
            }
        }
        else
        {
            turretRenderer.material.SetColor("_Main_Color", Color.red);
        }
    }

    public void CancelBuilding()
    {
        isBuilding = false;
        currentNeeds = null;
        Destroy(cubeTransform.gameObject);
        cubeTransform = null;
        return;
    }
    #endregion

    #region Inventory UI  
    private void CreateInventoryMenu() //Envanter'i prefablerden yaratma
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
    public void UpdateInventoryMenu() //Envanter �r�n bilgilerini g�ncelleme
    {
        int i = 0;
        foreach (var element in resourceIndices.Values)
        {
            UIAmount[i].text = element.ToString();
            i++;
        }
    }
    public void OpenInventoryMenu()
    {
        animator.SetBool("isShowing", true);
        isOpen = true;
        UpdateInventoryMenu();
    }

    public void CloseInventoryMenu()
    {
        animator.SetBool("isShowing", false);
        isOpen = false;
    }
    #endregion

    #region Inventory Operations
    public bool CheckResources(Dictionary<Resource1, int> neededResources) //Kaynak yeterlili�i kontrol�
    {

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
        currentNeeds = neededResources.ToDictionary(entry => entry.Key, entry => entry.Value);
        return true;
    }
    public void UseResources(Dictionary<Resource1, int> neededResources) //Kaynak harcan�m�
    {
        Dictionary<Resource1, int> tempHave = resourceIndices.ToDictionary(entry => entry.Key, entry => entry.Value);
        //De�eri G�ncelle
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
        currentNeeds.Clear();
    }
    #endregion
}

