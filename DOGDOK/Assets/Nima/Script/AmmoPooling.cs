using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoPooling : MonoBehaviour
{
    [SerializeField] GameObject ammoPrefab;
    [SerializeField] int creatAmount;

    // Start is called before the first frame update
    void Start()
    {
        LoadThePool();
    }

    void LoadThePool()
    {
        for (int i = 0; i < creatAmount; i++)
        {
            GameObject _ammo = Instantiate(ammoPrefab, transform);
            _ammo.SetActive(false);
            _ammo.transform.localPosition = Vector3.zero;
            _ammo.transform.localScale = Vector3.one;
            _ammo.GetComponent<Ammo>().ammoPooling = this;
        }
    }

    public void SpawnAmmo(float _distance)
    {
        GameObject _ammo = transform.GetChild(0).gameObject;

        Ammo _ammoComponent = _ammo.GetComponent<Ammo>();
        
        _ammoComponent.ammoLifeTime = _distance / _ammoComponent.speed;
        _ammo.SetActive(true);
        _ammo.transform.parent = null;
        
    }

    public void BackToThePool(GameObject _ammo)
    {
        _ammo.transform.parent = transform;
        _ammo.transform.localPosition = Vector3.zero;
        _ammo.transform.rotation = transform.rotation;
        _ammo.transform.localScale = Vector3.one;
        _ammo.SetActive(false);
    }


}
