using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotation : MonoBehaviour
{
    [SerializeField] float rotationLimit;
    [SerializeField] float rotationSensitivity;
    [SerializeField] GameObject cameraObject;
    [SerializeField] int width;
    [SerializeField] int height;

    [SerializeField] bool onVertical;
    [SerializeField] bool onHorizontal;

    
    // Start is called before the first frame update
    void Start()
    {
        width = Screen.width;
        height = Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 _mousePos = Input.mousePosition;
        float _mouseX = _mousePos.y - (height / 2);
        float _mouseY = (_mousePos.x - (width / 2)) * rotationSensitivity;

        float xDir = cameraObject.transform.eulerAngles.x;
        float zDir = cameraObject.transform.eulerAngles.z;
        float yDir = Mathf.Clamp( _mouseY, -rotationLimit, rotationLimit);

        
        cameraObject.transform.localEulerAngles= new Vector3(xDir,yDir,zDir);
    }
}
