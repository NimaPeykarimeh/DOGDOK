using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLightSystem : MonoBehaviour
{
    public GameObject _lightObject;
    public Light _light;
    public float maxLightIntensity;
    public GameObject player;
    public bool theLightIsOn;
    public float playerSpeed = 15;
    public float checkTimer;
    public float nextTimeToCheck;
    public float activationDistance = 15f;
    public float lightFullLength = 5f;
    public float distance;
    // Start is called before the first frame update
    void Start()
    {
        _lightObject = transform.GetChild(0).gameObject;
        _light = _lightObject.GetComponent<Light>();
        maxLightIntensity = _light.intensity;
        player = GameObject.FindGameObjectWithTag("Player"); // Adjust the tag
    }


    void CheckPlayerDistance()
    {
        checkTimer = 0;
        distance = Vector3.Distance(transform.position, player.transform.position) - activationDistance;
        
        if (distance <= 0)
        {
            theLightIsOn = true;
            _lightObject.SetActive(true);
        }
        else
        {
            _lightObject.SetActive(false);
            theLightIsOn = false;
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
        if (theLightIsOn)
        {
            float _lightRatio = Mathf.Clamp01(distance / (-lightFullLength));
            float _newValue = Mathf.Lerp(0, maxLightIntensity, _lightRatio);
            float _oldValue = _light.intensity;
            if(_oldValue!= _newValue)
            {
                _light.intensity = _newValue;
            }
        }
    }
}
