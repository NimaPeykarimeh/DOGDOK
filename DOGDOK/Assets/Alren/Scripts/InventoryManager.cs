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
    [SerializeField] private CollectibleManager CollectibleManager;

    private void Awake()
    {
        InventorySlots = new int[6];
        Array.Clear(InventorySlots, 0, 6);
    }
    void Start()
    {
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
            for (int i = 0; i < InventorySlots.Length; i++)
            {
                UIAmount[i].text = InventorySlots[i].ToString();
            }
            animator.SetBool("isShowing", true);
            isInventoryOpen = true;
        }
        else if (isInventoryOpen) // Envanter A��k Duruyorsa.
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
    public void UseResources(string resourceName, int usageAmount) //kayna��n t�r�n� say� cinsinden ve miktar�n� giriniz.
    {
        //if (CollectibleManager.resourceIndices.TryGetValue(resourceName, out int resourceType)) //kayna��n ismine g�re kaynak t�r�n� d�nd�r�yor.
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
