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
        if (isInventoryOpen && Input.GetKeyDown(KeyCode.I)) // Envanter A��kken Kapama
        {
            animator.SetBool("isShowing", false);
            isInventoryOpen = false;
        }
        else if (Input.GetKeyDown(KeyCode.I)) // Envanter Kapal�yken A�ma
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
        else if (isInventoryOpen) // Envanter A��k Duruyorsa.
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

    //Tab'a basarak silah se�me sekmesi
    //WorkStation yan�nda E'ye bas�nca tablet a��ls�n, butona bas�l�nca grid sisteme ge�sin
    //Build, Gridleri takip etsin. Grid bo� mu kontrol� yapan method.
    public void UseResources(Dictionary<Resource1,int> neededResources) //kayna��n t�r�n� say� cinsinden ve miktar�n� giriniz.
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
