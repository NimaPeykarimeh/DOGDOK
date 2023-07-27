using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public AmmoPooling ammoPooling;
    [SerializeField] Rigidbody rb;

    [SerializeField] float speed;

    [SerializeField] float ammoLifeTime;
    [SerializeField] float ammoTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()//fix scaling
    {
        ammoTimer = 0;
        rb.velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        ammoTimer += Time.deltaTime;
        if (ammoTimer >= ammoLifeTime)
        {
            ammoPooling.BackToThePool(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.up * speed * Time.fixedDeltaTime,ForceMode.Impulse);
    }
}
