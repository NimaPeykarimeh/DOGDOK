using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [HideInInspector]
    public AmmoPooling ammoPooling;

    [HideInInspector] 
    public float ammoLifeTime;

    float ammoTimer;
    TrailRenderer trail;

    public float speed;

    void Start()
    {

    }

    private void OnEnable()
    {
        ammoTimer = 0;
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
