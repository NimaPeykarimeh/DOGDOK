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
        if (isInventoryOpen && Input.GetKeyDown(KeyCode.I)) // Envanter A��kken Kapama
        {
            animator.SetBool("isShowing", false);
            isInventoryOpen = false;
        }
        else if (Input.GetKeyDown(KeyCode.Tab)) // Envanter Kapal�yken A�ma
        {
            for (int i = 0; i < InventorySlots.Length; i++)
            {
                UIAmount[i].text = InventorySlots[i].ToString();
            }
            animator.SetBool("isShowing", true);
            isInventoryOpen = true;
        }
        else if(isInventoryOpen) // Envanter A��k Duruyorsa.
        {
            for (int i = 0; i < InventorySlots.Length; i++)
            {
                UIAmount[i].text = InventorySlots[i].ToString();
            }
        }
    }
    //TODO
    //Gather Resources method olarak Ekle
    //Switch Case'i resource gatherlamay� ekle
    //Kaynaklar� Dictionary'e �evir

    //Tab'a basarak silah se�me sekmesi
    //WorkStation yan�nda E'ye bas�nca tablet a��ls�n, butona bas�l�nca grid sisteme ge�sin
    //Build, Gridleri takip etsin. Grid bo� mu kontrol� yapan method.
    public void UseResources(int resourceType, int usageAmount) //kayna��n t�r�n� say� cinsinden ve miktar�n� giriniz.
    {
        int lastingResource = InventorySlots[resourceType] - usageAmount;
        if (lastingResource < 0)
        {
            Console.WriteLine("Can't");
        }
        else InventorySlots[resourceType] = lastingResource;
    }
}
