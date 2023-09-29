using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergyController : MonoBehaviour
{
    [Header("References")]
    PlayerController playerController;
    [SerializeField] bool isInEnergyArea;
    [SerializeField] Image energyImage;
    [Header("EnergyInfo")]
    [SerializeField] float maxEnergy = 500;
    public float currentEnergy;
    [SerializeField] bool isFilling;
    [SerializeField] float fillDuration = 0.5f;
    [SerializeField] float fillSpeed;


    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    private void Start()
    {
        currentEnergy = maxEnergy;   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnergySource"))
        {
            if (other.name == "InteractionArea")
            {
                playerController.SetInteractionText(true, "Press F To Fill Energy");
                isInEnergyArea = true;
            }
        }
    }

    public bool UseEnergy(float _amount)
    {
        if (currentEnergy - _amount >= 0)
        {
            currentEnergy -= _amount;
            energyImage.fillAmount = currentEnergy / maxEnergy;
            return true;
        }
        else
        {
            Debug.Log("Out Of Energy");
        }
        return false;
    }

    public float UseAllEnergy()
    {
        float _currentEnergy = currentEnergy;
        currentEnergy = 0;
        energyImage.fillAmount = currentEnergy;
        return _currentEnergy;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnergySource"))
        {
            if (other.name == "InteractionArea")
            {
                playerController.SetInteractionText(false);
                isInEnergyArea = false;
            }
        }
    }

    public void FillEnergy()
    {
        isFilling = true;
        fillSpeed = (maxEnergy - currentEnergy) / fillDuration;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isInEnergyArea)
        {
            FillEnergy();
        }
        if (isFilling)
        {
            currentEnergy = Mathf.MoveTowards(currentEnergy,maxEnergy,fillSpeed * Time.deltaTime);
            isFilling = currentEnergy != maxEnergy;
            energyImage.fillAmount = currentEnergy / maxEnergy;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            UseEnergy(50);
        }
    }
}
