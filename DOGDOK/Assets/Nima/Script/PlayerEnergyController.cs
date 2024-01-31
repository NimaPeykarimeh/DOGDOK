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
    bool isFilling;
    [SerializeField] float fillDuration = 0.5f;
    [SerializeField] float fillSpeed;
    bool isUsing;
    float targetEnergy;
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
                DisplayText.Instance.ShowText(true, "Press 'F' To Fill");
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
            DisplayText.Instance.ShowText(true, "Out Of Energy\r\n(Use Generetor To Refill)",2f,DisplayText.TextType.Warning);
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
                DisplayText.Instance.ShowText(false);
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
