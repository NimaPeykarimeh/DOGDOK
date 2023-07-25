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
    // Start is called before the first frame update
    void Start()
    {
        isInventoryOpen = false;
        InventorySlots = new int[6];
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            InventorySlots[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInventoryOpen && Input.GetKeyDown(KeyCode.Tab)) // Envanter Açýkken Kapama
        {
            animator.SetBool("isShowing", false);
            isInventoryOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.Tab)) // Envanter Kapalýyken Açma
        {
            for (int i = 0; i < InventorySlots.Length; i++)
            {
                UIAmount[i].text = InventorySlots[i].ToString();
            }
            animator.SetBool("isShowing", true);
            isInventoryOpen = false;
        }
        else if (isInventoryOpen) // Envanter Açýk Duruyorsa.
        {
            for (int i = 0; i < InventorySlots.Length; i++)
            {
                UIAmount[i].text = InventorySlots[i].ToString();
            }
        }
    }

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
