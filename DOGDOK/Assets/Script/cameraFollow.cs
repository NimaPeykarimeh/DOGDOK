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

    private void Start()
    {
        currentFollowSpeed = defaultFollowSpeed;
        playerMovement = player.GetComponent<CharacterMovement>();
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
    }
}
