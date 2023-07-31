using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] List<EnemyTeleportManager> enemiesTeleport = new List<EnemyTeleportManager>();

    [SerializeField] int teleportLimit;
    [SerializeField] float coolDown;
    [SerializeField] float coolDownTimer;

    [SerializeField] float teleportingDuration;
    [SerializeField] float teleportingTimer;

    [SerializeField] GameObject teleportingCenter;
    [SerializeField] float teleportingZ;
    [SerializeField] float teleportingX;

    [SerializeField] bool isReadyToTeleport;

    [SerializeField] bool isEntered;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemiesTeleport.Count ==0)
            {
                teleportingTimer = 0;   
                isEntered = true;

            }
            enemiesTeleport.Add(other.gameObject.GetComponent<EnemyTeleportManager>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesTeleport.Remove(other.gameObject.GetComponent<EnemyTeleportManager>());
            isEntered = enemiesTeleport.Count != 0;
        }
    }

    void Teleport()
    {
        if (enemiesTeleport.Count > 0)
        {
            for (int i = 0; i < teleportLimit; i++)
            {
                float positionToTeleportX = Random.Range(teleportingCenter.transform.position.x - teleportingX,teleportingCenter.transform.position.x + teleportingX);
                float positionToTeleportZ = Random.Range(teleportingCenter.transform.position.z - teleportingZ, teleportingCenter.transform.position.z + teleportingZ);

                Vector3 positonToTeleport = new Vector3(positionToTeleportX,0,positionToTeleportZ);

                if (enemiesTeleport.Count > 0)
                {
                    enemiesTeleport[0].Teleport(positonToTeleport);
                    enemiesTeleport.RemoveAt(0);
                    if (enemiesTeleport.Count == 0)
                    {
                        isEntered = false;
                        break;
                    }
                }
                else
                {
                    Debug.Log("No More Enemy");
                }
            }
            isReadyToTeleport = false;
            coolDownTimer = 0;
            teleportingTimer = 0;
        }
    }

    private void Update()
    {
        coolDownTimer += Time.deltaTime;
        if (coolDownTimer >= coolDown)
        {
            isReadyToTeleport = true;
        }
        if (isEntered && isReadyToTeleport)
        {
            teleportingTimer += Time.deltaTime;
            if (teleportingTimer >= teleportingDuration)
            {
                Teleport();
            }
        }
    }
}
