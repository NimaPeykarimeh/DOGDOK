using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] Vector3 maxZoomAngle;
    [SerializeField] Vector3 maxZoomDistance;

    [SerializeField] Vector3 minZoomAngle;
    [SerializeField] Vector3 minZoomDistance;
    [SerializeField] Vector3 currentZoomDistance;
    [SerializeField] Vector3 currentZoomAngle;

    [Header("Scroll Value")]
    [SerializeField] float scrollLimit = 10f;
    [SerializeField] float scrollValue;
    [SerializeField] float currentValue;
    [SerializeField] float t;

    [Header("ZoomTimer")]
    Vector3 zoomStartPosition;
    Vector3 zoomStartRotation;

    [SerializeField] bool isZooming;
    [SerializeField] float zoomTimer;
    [SerializeField] float zoomDuration;
    void Start()
    {
        
    }
    void SaveMinZoom()
    {

    }
    private void GetCurrentZoom(float zoomAmount)
    {
        zoomTimer = 0;
        isZooming = true;
        t = zoomAmount/scrollLimit;
        currentZoomDistance = Vector3.Lerp(minZoomDistance,maxZoomDistance, t);
        currentZoomAngle = Vector3.Lerp(minZoomAngle,maxZoomAngle,t);
        zoomStartPosition = transform.localPosition;
        zoomStartRotation = transform.localEulerAngles;
        //transform.localPosition = currentZoomDistance;
        //transform.localEulerAngles = currentZoomAngle;
    }
    
    // Update is called once per frame
    void Update()
    {
        //t = Mathf.Lerp(0,100,n);
        scrollValue = Input.mouseScrollDelta.y;
        if (scrollValue != 0)
        {
            currentValue += scrollValue;
            currentValue = Mathf.Clamp(currentValue, 0, scrollLimit);
            GetCurrentZoom(currentValue);
        }
        if (isZooming)
        {
            zoomTimer += Time.deltaTime;

            float ratio = (zoomTimer / zoomDuration);
            transform.localPosition = Vector3.Lerp(zoomStartPosition, currentZoomDistance,ratio);
            transform.localEulerAngles = Vector3.Lerp(zoomStartRotation,currentZoomAngle,ratio);

            if (zoomTimer>=zoomDuration)
            {
                isZooming = false;
            }
        }
    }
}
