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

    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private GameObject resBlockPrefab;
    [SerializeField] private List<Resource1> resources = new();


    [HideInInspector] public Dictionary<Resource1, int> resourceIndices = new ();

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
        isInventoryOpen = false;
    }

    void Update()
    {
        if (isInventoryOpen && Input.GetKeyDown(KeyCode.I)) // Envanter A��kken Kapama
        {
            animator.SetBool("isShowing", false);
            isInventoryOpen = false;
        }
        else if (Input.GetKeyDown(KeyCode.I)) // Envanter Kapal�yken A�ma
        {
            UpdateInventoryMenu();
            animator.SetBool("isShowing", true);
            isInventoryOpen = true;
        }
        else if (isInventoryOpen) // Envanter A��k Duruyorsa.
        {
            UpdateInventoryMenu();
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

    //Tab'a basarak silah se�me sekmesi
    //WorkStation yan�nda E'ye bas�nca tablet a��ls�n, butona bas�l�nca grid sisteme ge�sin
    //Build, Gridleri takip etsin. Grid bo� mu kontrol� yapan method.
    public bool UseResources(Dictionary<Resource1, int> neededResources) //kayna��n t�r�n� say� cinsinden ve miktar�n� giriniz.
    {
        //Kaynak yeterlili�i kontrol�
        foreach (var need in neededResources)
        {
            foreach (var have in resourceIndices)
            {
                if (have.Key == need.Key)
                {
                    if(have.Value - need.Value < 0)
                    {
                        return false;
                    }
                }
            }
        }
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
        return true;
    }
}
