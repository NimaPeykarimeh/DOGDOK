using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretGroundedControl : MonoBehaviour
{
    [HideInInspector] public bool isGrounded;

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Ground"))
        {
            print("grounded");
            isGrounded = true;
        }
        else
        {
            print("notgrounded");
            isGrounded = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }
}
