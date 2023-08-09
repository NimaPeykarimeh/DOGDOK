using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretGroundedControl : MonoBehaviour
{
    [HideInInspector] public bool isGrounded;
    private void OnTriggerStay(Collider collision) => isGrounded = collision.CompareTag("Ground") ? true : false;
    private void OnTriggerExit(Collider other) => isGrounded = false;
}
