using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    private PlayerController playerController;
    private List<TextMeshProUGUI> UIAmount = new();
    private bool buildingSelected;
    private Build1 currentBuild;
    private Transform cubeTransform;
    private Dictionary<Resource1, int> currentNeeds;
    //private Renderer turretRenderer;
    private BoxCollider turretCollider;
    private bool isAired;

    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private GridDisplay GridDisplay;
    [SerializeField] private BuildableArea BuildableArea;
    private GameObject turretPrefab;
    [SerializeField] private GameObject InventoryPanel;
    public GameObject Tablet;
    [SerializeField] private GameObject resBlockPrefab;
    [SerializeField] private List<Resource1> resources = new();
    [SerializeField] private byte buildDistance;

    [SerializeField] LayerMask turretPlaceHitLayers;

    [HideInInspector] public bool isBuilding;
    [HideInInspector] public bool isOpen;
    [HideInInspector] public Dictionary<Resource1, int> resourceIndices = new();

    private void Awake() //resource id'leri otomatik atanýyor.
    {
        int i = 0;
        playerController = FindAnyObjectByType<PlayerController>();
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
        Tablet.SetActive(false);
    }

    void Update()
    {
        if (buildingSelected) // Turret'ý yarat ve location'ýný al.
        {
            cubeTransform = CreateTurret();
        }
        else if (cubeTransform != null) // Mouse'u takip ederek Turret'ýn koyulacaðý yere bak ve güncelle.
        {
            TrackMouseForBuilding();
        }
    }

    #region Turret Building
    public void SetCurrentBuild(Build1 build)
    {
        buildingSelected = true;
        currentBuild = build;
        turretPrefab = build.turretPrefab;
    }
    private Transform CreateTurret()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //kamerayý cachle daha optimize.
        isBuilding = true;
        playerController.ChangePlayerState(PlayerController.PlayerStates.Building);
        if (AreaManager.areasMesh.Count > 0)
        {
            foreach (var area in AreaManager.areasMesh)
            {
                if(area != null)
                {
                    area.GetComponent<MeshRenderer>().enabled = true;
                }

            }
        }

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 positionToPlace = hit.point;

            if (hit.collider.gameObject.CompareTag("Ground"))
            {

                positionToPlace = PlaceObjectOnGrid(positionToPlace, currentBuild.buildingSize);
                //turretPrefab.transform.localScale = GridDisplay.cellSize * currentBuild.buildingSize; //scale'i !!!!!!
                positionToPlace.y += turretPrefab.transform.localScale.y / 2; //küp yüksekliði
                Transform turretTransform = Instantiate(turretPrefab, positionToPlace, transform.rotation).GetComponent<Transform>();
                //turretRenderer = turretTransform.GetComponent<TurretNullControl>().Renderer;
                turretCollider = turretTransform.GetComponent<BoxCollider>();
                buildingSelected = false;

                for (int i = 0; i < turretTransform.childCount; i++)
                {
                    if (turretTransform.GetChild(i).TryGetComponent<Collider>(out Collider _collider))
                    {
                        _collider.enabled = false;
                    }
                }

                return turretTransform;
            }
        }
        return null;
    }
    private Vector3 PlaceObjectOnGrid(Vector3 position, Vector3 size)
    {
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

        Vector3 snappedPosition = new Vector3(xPosition, 0, zPosition);//sonra zemin konumu alýrýz
        return snappedPosition;
    }
    private void TrackMouseForBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 positionToPlace;
        if (Physics.Raycast(ray, out RaycastHit hit, buildDistance, GroundLayer))
        {
            positionToPlace = hit.point;
            isAired = false;
        }
        else
        {
            isAired = true;
            positionToPlace = ray.GetPoint(buildDistance);
            ray = new Ray(positionToPlace, Vector3.down);
            if (Physics.Raycast(ray, out hit, GroundLayer))
            {
                positionToPlace = hit.point;
            }
        }
        //positionToPlace.y = 0;
        cubeTransform.position = PlaceObjectOnGrid(positionToPlace, currentBuild.buildingSize);
        if (!isAired && BuildableArea.CheckBuildableArea(cubeTransform.position) && !Physics.CheckBox(cubeTransform.position, currentBuild.buildingSize / 2, turretCollider.transform.rotation, turretPlaceHitLayers))
        {
            cubeTransform.gameObject.GetComponent<TurretNullControl>().TurretColorSelector(true);
            if (Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetKeyDown(KeyCode.E) && !Input.GetKeyDown(KeyCode.Escape))
            {
                UseResources(currentNeeds);
                cubeTransform.gameObject.GetComponent<TurretController>().StartGenerating();
                //cubeTransform.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                cubeTransform.gameObject.GetComponent<TurretNullControl>().enabled = false;
                isBuilding = false;

                if (AreaManager.areasMesh.Count > 0)
                {
                    foreach (var area in AreaManager.areasMesh)
                    {
                        if (area != null)
                            area.GetComponent<MeshRenderer>().enabled = false;
                    }
                }

                cubeTransform = null;
                return;
            }
        }
        else
        {
            cubeTransform.gameObject.GetComponent<TurretNullControl>().TurretColorSelector(false);
        }
    }
    private void OnDrawGizmos()
    {
        // Set the color of the Gizmos
        Gizmos.color = Color.green;

        // Calculate the world space position of the CheckBox
        Vector3 checkBoxPosition = cubeTransform.position;

        // Draw the CheckBox using Gizmos
        Gizmos.DrawWireCube(checkBoxPosition, currentBuild.buildingSize);
    }
    public void CancelBuilding()
    {
        isBuilding = false;
        playerController.ChangePlayerState(PlayerController.PlayerStates.Basic);
        if (AreaManager.areasMesh.Count > 0)
        {
            foreach (var area in AreaManager.areasMesh)
            {
                if (area != null)
                    area.GetComponent<MeshRenderer>().enabled = false;
            }
        }

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
            //resBlock.transform.Find("resText").GetComponent<TextMeshProUGUI>().text = element.resourceName;
            resBlock.transform.Find("resImage").GetComponent<Image>().sprite = element.resourceImage;
            UIAmount.Add(resBlock.transform.Find("resAmountText").GetComponent<TextMeshProUGUI>());
            UIAmount[i].text = "X0";
            i++;
        }
        InventoryPanel.SetActive(false);
        isOpen = false;
    }
    public void UpdateInventoryMenu() //Envanter ürün bilgilerini güncelleme
    {
        int i = 0;
        foreach (var element in resourceIndices.Values)
        {
            UIAmount[i].text ="X" + element.ToString();
            i++;
        }
    }
    public void OpenInventoryMenu()
    {
        isOpen = true;
        UpdateInventoryMenu();
        InventoryPanel.SetActive(true);
    }

    public void CloseInventoryMenu()
    {
        isOpen = false;
        InventoryPanel.SetActive(false);
    }
    #endregion

    #region Inventory Operations
    public bool CheckResources(Dictionary<Resource1, int> neededResources) //Kaynak yeterliliði kontrolü
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
    public void UseResources(Dictionary<Resource1, int> neededResources) //Kaynak harcanýmý
    {
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
        currentNeeds.Clear();
    }
    public void AddResources(Dictionary<Resource1, int> addingResources) //Kaynak harcanýmý
    {
        Dictionary<Resource1, int> tempHave = resourceIndices.ToDictionary(entry => entry.Key, entry => entry.Value);
        //Deðeri Güncelle
        foreach (var add in addingResources)
        {
            foreach (var have in tempHave)
            {
                if (have.Key == add.Key)
                {
                    resourceIndices[have.Key] += add.Value;
                }
            }
        }
    }

    #endregion
}

