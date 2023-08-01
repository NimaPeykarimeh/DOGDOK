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

        if (currentBuild != null)
        {
            CreateTurret();
        }
    }

    private void CreateTurret()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 positionToPlace = hit.point;

                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    positionToPlace = PlaceObjectOnGrid(positionToPlace);
                    turretPrefab.transform.localScale = GridDisplay.cellSize * currentBuild.buildingSize; //scale'i !!!!!!
                    positionToPlace.y += turretPrefab.transform.localScale.y / 2; //küp yüksekliði
                    Instantiate(turretPrefab, positionToPlace, transform.rotation);
                    currentBuild = null;
                }
            }
        }

        // Turretlarý grid'e tam oturt.
        // Zombieleri takip eden silah namlusu
        // NULL kontrolü yap.
        // Player'dan uzaksa yapamasýn.

        Vector3 PlaceObjectOnGrid(Vector3 position)
        {
            int x = Mathf.RoundToInt(position.x / GridDisplay.cellSize);
            int y = Mathf.RoundToInt(position.y / GridDisplay.cellSize);
            int z = Mathf.RoundToInt(position.z / GridDisplay.cellSize);
            Vector3 snappedPosition = new Vector3(x * GridDisplay.cellSize - GridDisplay.cellSize / 2
                , y * GridDisplay.cellSize
                , z * GridDisplay.cellSize - GridDisplay.cellSize / 2);
            return snappedPosition;
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

    //TODO

    //Tab'a basarak silah seçme sekmesi
    //WorkStation yanýnda E'ye basýnca tablet açýlsýn, butona basýlýnca grid sisteme geçsin
    //Build, Gridleri takip etsin. Grid boþ mu kontrolü yapan method.
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

    public void setCurrentBuild(Build1 build)
    {
        currentBuild = build;
    }
}
