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
    [HideInInspector] public int[] InventorySlots;
    [SerializeField] private TextMeshProUGUI[] UIAmount = new TextMeshProUGUI[6];

    void Start()
    {
        isInventoryOpen = false;
        InventorySlots = new int[6];
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            InventorySlots[i] = 0;
        }
    }

    void Update()
    {
        if (isInventoryOpen && Input.GetKeyDown(KeyCode.I)) // Envanter Açýkken Kapama
        {
            animator.SetBool("isShowing", false);
            isInventoryOpen = false;
        }
        else if (Input.GetKeyDown(KeyCode.Tab)) // Envanter Kapalýyken Açma
        {
            for (int i = 0; i < InventorySlots.Length; i++)
            {
                UIAmount[i].text = InventorySlots[i].ToString();
            }
            animator.SetBool("isShowing", true);
            isInventoryOpen = true;
        }
        else if(isInventoryOpen) // Envanter Açýk Duruyorsa.
        {
            for (int i = 0; i < InventorySlots.Length; i++)
            {
                UIAmount[i].text = InventorySlots[i].ToString();
            }
        }
    }
    //TODO
    //Gather Resources method olarak Ekle
    //Switch Case'i resource gatherlamayý ekle
    //Kaynaklarý Dictionary'e çevir

    //Tab'a basarak silah seçme sekmesi
    //WorkStation yanýnda E'ye basýnca tablet açýlsýn, butona basýlýnca grid sisteme geçsin
    //Build, Gridleri takip etsin. Grid boþ mu kontrolü yapan method.
    public void UseResources(int resourceType, int usageAmount) //kaynaðýn türünü sayý cinsinden ve miktarýný giriniz.
    {
        int lastingResource = InventorySlots[resourceType] - usageAmount;
        if (lastingResource < 0)
        {
            Console.WriteLine("Can't");
        }
        else InventorySlots[resourceType] = lastingResource;
    }
}
