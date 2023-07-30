using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    private bool isInventoryOpen;
    [SerializeField] private Animator animator;
    [SerializeField] private List<TextMeshProUGUI> UIAmount = new();
    [SerializeField] private CollectibleManager CollectibleManager;

    private void Awake()
    {

    }
    void Start()
    {
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
            int i = 0;
            foreach (var element in CollectibleManager.resourceIndices)
            {
                UIAmount[i].text = CollectibleManager.resourceIndices[element.Key].ToString();
                i++;
            }
            animator.SetBool("isShowing", true);
            isInventoryOpen = true;
        }
        else if (isInventoryOpen) // Envanter Açýk Duruyorsa.
        {
            int i = 0;
            foreach (var element in CollectibleManager.resourceIndices)
            {
                UIAmount[i].text = CollectibleManager.resourceIndices[element.Key].ToString();
                i++;
            }
        }
    }
    //TODO

    //Tab'a basarak silah seçme sekmesi
    //WorkStation yanýnda E'ye basýnca tablet açýlsýn, butona basýlýnca grid sisteme geçsin
    //Build, Gridleri takip etsin. Grid boþ mu kontrolü yapan method.
    public void UseResources(Dictionary<Resource1,int> neededResources) //kaynaðýn türünü sayý cinsinden ve miktarýný giriniz.
    {
        //if (CollectibleManager.resourceIndices.TryGetValue(resourceName, out int resourceType)) //kaynaðýn ismine göre kaynak türünü döndürüyor.
        //{
        //    int lastingResource = InventorySlots[resourceType] - usageAmount;
        //    if (lastingResource < 0)
        //    {
        //        Debug.Log("Can't");
        //    }
        //    else
        //    {
        //        InventorySlots[resourceType] = lastingResource;
        //    }
        //}

    }
}
