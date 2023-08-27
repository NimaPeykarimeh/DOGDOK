using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DayNightCycle : MonoBehaviour
{
    [Range(0, 24)]
    [SerializeField] float dayTime;
    [SerializeField] int dayTimeDurationMin = 1;
    [SerializeField] Transform sunTransform;
    [SerializeField] HDAdditionalLightData[] sunLight;
    [SerializeField] float[] maxLight;
    [SerializeField] float[] minLight;
    [SerializeField] float dayLigthPow;
    [SerializeField] List<Material> shaderMaterials;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void ChangeDayValues(float _dayTime)
    {
        float _sunAngle = Mathf.Lerp(-90,270,_dayTime/24);
        sunTransform.rotation = Quaternion.Euler(_sunAngle,-30f,0f);
        float _sunIntensity = Mathf.Pow((0.5f - Mathf.Abs((0.5f - (_dayTime/24f)))) / 0.5f, dayLigthPow);
        for (int i = 0; i < sunLight.Length; i++)
        {
            sunLight[i].intensity = Mathf.Lerp(minLight[i], maxLight[i], _sunIntensity);
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
        ChangeDayValues(dayTime);
        dayTime += Time.fixedDeltaTime * (24f / ((float)dayTimeDurationMin * 60f));
        if (dayTime >= 24)
        {
            dayTime = 0;
        }
    }
}
