using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DayNightCycle : MonoBehaviour
{
    [Range(0, 24)]
    [SerializeField] float dayTime;
    [SerializeField] int dayCounter = 0;
    [SerializeField] float sunRiseHour = 6;
    [SerializeField] float sunSetHour = 18;
    [SerializeField] int dayTimeDurationMin = 1;
    [SerializeField] Transform sunTransform;
    [SerializeField] HDAdditionalLightData[] sunLight;
    [SerializeField] float[] maxLight;
    [SerializeField] float[] minLight;
    [SerializeField] float dayLigthPow;
    [SerializeField] List<Material> shaderMaterials;
    [SerializeField] List<AutoLightSystem> autoLightList;
    [SerializeField] List<float> maxIntensityList;
    [SerializeField] float testValue;
    public bool isDay;

    [SerializeField] bool doesCycle;
    // Start is called before the first frame update
    void Start()
    {
        foreach (AutoLightSystem _autoLight in FindObjectsOfType<AutoLightSystem>())//findAllTheAreaLightsInScene
        {
            autoLightList.Add(_autoLight);
            maxIntensityList.Add(_autoLight.targetLightIntensity);
        }
        ChangeDayValues(dayTime);
        ChangeAreaLightValues();
    }

    void ChangeAreaLightValues()
    {
        foreach (AutoLightSystem _autoLight in autoLightList)//bir þekilde çalýþýyor, dokunma
        {
            float _lightValue =  6 - Mathf.Clamp(Mathf.Abs(12 - dayTime), 0,6);// - (sunSetHour - sunRiseHour)
            testValue =  Mathf.Clamp(_lightValue,0.1f,5) / 5;//between 11 and 13 is max lights
            _autoLight.targetLightIntensity = Mathf.Lerp(0, _autoLight.maxLightIntensity, testValue);
        }
    }

    void ChangeDayValues(float _dayTime)
    {
        float _sunAngle = Mathf.Lerp(-90,270,_dayTime/24);
        sunTransform.rotation = Quaternion.Euler(_sunAngle, -41.5f, 0f);
        float _sunIntensity = Mathf.Pow((0.5f - Mathf.Abs((0.5f - (_dayTime/24f)))) / 0.5f, dayLigthPow);
        for (int i = 0; i < sunLight.Length; i++)
        {
            sunLight[i].intensity = Mathf.Lerp(minLight[i], maxLight[i], testValue);
        }

        foreach (Material _mat in shaderMaterials)
        {
            float _brighnes = _mat.GetFloat("_Color_Brightness");
            float _newBrighness = Mathf.Lerp(0.01f, 1000, _sunIntensity);

            //_mat.SetFloat("_Color_Brightness", _newBrighness);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (doesCycle)
        {
            ChangeDayValues(dayTime);
            ChangeAreaLightValues();
            dayTime += Time.fixedDeltaTime * (24f / ((float)dayTimeDurationMin * 60f));
            if (dayTime >= 6 && !isDay && dayTime <= 18)
            {
                isDay = true;
                Debug.Log("DayTimeChanged");
            }
            if ((dayTime > 18 || dayTime < 6) && isDay)
            {
                isDay = false;
                Debug.Log("DayTimeChanged");
            }
            if (dayTime >= 24)
            {
                dayTime = 0;
                dayCounter++;
            }
        }
        
    }
}
