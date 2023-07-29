using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public AmmoPooling ammoPooling;
    [SerializeField] Rigidbody rb;

    public float speed;

    public float ammoLifeTime;
    [SerializeField] float ammoTimer;
    [SerializeField] TrailRenderer trail;

    Vector3 startPos;
    Vector3 endPos;

    void Start()
    {

    }

    private void OnEnable()
    {
        ammoTimer = 0;
        //transform.localScale = Vector3.one;
        trail.Clear();

    }

    // Update is called once per frame
    void Update()
    {
        ammoTimer += Time.deltaTime;
        if (ammoTimer >= ammoLifeTime)
        {

            ammoPooling.BackToThePool(this.gameObject);
        }
        transform.Translate(-transform.forward * speed * Time.deltaTime);
    }

}
