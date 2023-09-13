using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLightSystem : MonoBehaviour
{
    public GameObject _light;
    public GameObject player;
    public float playerSpeed = 15;
    public float checkTimer;
    public float nextTimeToCheck;
    public float activationDistance = 5f;
    public float distance;
    // Start is called before the first frame update
    void Start()
    {
        _light = transform.GetChild(0).gameObject;
        player = GameObject.FindGameObjectWithTag("Player"); // Adjust the tag
    }

    private void OnTriggerEnter(Collider other)
    {
        _light.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        _light.SetActive(false);
    }

    void CheckPlayerDistance()
    {
        checkTimer = 0;
        distance = Vector3.Distance(transform.position, player.transform.position) - activationDistance;
        
        if (distance <= 0)
        {
            _light.SetActive(true);
        }
        else
        {
            _light.SetActive(false);
            nextTimeToCheck = (distance / playerSpeed) ;
        }
    }

    private void Update()
    {
        checkTimer += Time.deltaTime;
        if (checkTimer >= nextTimeToCheck)
        {
            CheckPlayerDistance();
        }
    }
}
