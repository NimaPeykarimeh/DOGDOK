using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    CharacterMovement playerMovement;

    [SerializeField] float defaultFollowSpeed;
    [SerializeField] float currentFollowSpeed;
    public float maxDistance;

    //[SerializeField] int maxRotation = 30;
    //[SerializeField] float rotationSensitivity;
    //[SerializeField] int screenResX;
    private void Start()
    {
        currentFollowSpeed = defaultFollowSpeed;
        playerMovement = player.GetComponent<CharacterMovement>();
        //screenResX = Screen.width;
    }



    private void LateUpdate()
    {
        if (Vector3.Distance(transform.position,player.transform.position) >= maxDistance)
        {
            currentFollowSpeed = playerMovement.movementSpeed;
        }
        else
        {
            currentFollowSpeed = defaultFollowSpeed;
        }
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, currentFollowSpeed * Time.deltaTime);
        //float turnRotation = Mathf.Clamp( (Input.mousePosition.x - (screenResX / 2)) * rotationSensitivity, -maxRotation,maxRotation);
        //transform.eulerAngles = new Vector3(0,turnRotation ,0);
    }
}
