using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFireController : MonoBehaviour
{
    public List<Transform> EnemyList;
    TurretController turretController;

    [SerializeField] private Transform _bodyTransform;
    //[SerializeField] private Transform _headTransform;
    [SerializeField] private Transform _firePoint;
    //[SerializeField] float offsetY;
    [SerializeField] float RotateSpeed;

    [Header("BulletProps")]
    [SerializeField] private AmmoPooling ammoPooling;
    [SerializeField] List<GameObject> pooledBullets;
    [SerializeField] Transform BulletParent;
    [SerializeField] GameObject Bullet;
    [SerializeField] int BulletCount;

    [SerializeField] LayerMask shootingLayer;
    public float FireCooldownTime;
    public int Damage;
    public float ShootingRange;

    private float _fireTimer;
    private int _bulletIndex;

    [Header("EnergySettings")]
    public bool canShoot = false;

    void Start()
    {
        turretController = GetComponent<TurretController>();
        _bulletIndex = 0;
        canShoot = false;
        pooledBullets = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < BulletCount; ++i)
        {
            tmp = Instantiate(Bullet);
            tmp.SetActive(false);
            tmp.transform.SetParent(BulletParent);
            tmp.transform.localPosition = Vector3.zero;
            tmp.GetComponent<Ammo>().ammoPooling = ammoPooling;
            pooledBullets.Add(tmp);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot && turretController.readyToUse)
        {
            if (EnemyList.Count != 0)
            {
                if (EnemyList[0].gameObject.activeSelf)
                    ShootTheEnemy(EnemyList[0]);
                else
                    EnemyList.RemoveAt(0);
            }

            if (_fireTimer < FireCooldownTime + 1f)
                _fireTimer += Time.deltaTime;
        }

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        EnemyList.Add(other.transform);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        EnemyList.Remove(other.transform);
    //    }
    //}

    public void RemoveEnemyFromList(Transform Enemy)
    {
        EnemyList.Remove(Enemy);
    }

    public void ShootTheEnemy(Transform Target)
    {
        RotateBodyToEnemy(Target);
        //RotateHeadToEnemy(Target);

        if (_fireTimer > FireCooldownTime)
        {
            FireToEnemy(Target);
            _fireTimer = 0;
        }
    }

    public void RotateBodyToEnemy(Transform Target)
    {
        Vector3 targetDirectionXZ = Target.position - _bodyTransform.position;
        targetDirectionXZ.y = _bodyTransform.position.y;
        float angleY = Mathf.Atan2(targetDirectionXZ.x, targetDirectionXZ.z) * Mathf.Rad2Deg;

        _bodyTransform.rotation = Quaternion.Slerp(_bodyTransform.rotation, Quaternion.Euler(0f, angleY, 0f), RotateSpeed * Time.deltaTime);
    }

    //public void RotateHeadToEnemy(Transform Target)
    //{
    //    Vector2 targetPosXZ = new Vector2(Target.position.x, Target.position.z);
    //    Vector2 headPosXZ = new Vector2(_headTransform.position.x, _headTransform.position.z);
    //    float distanceXZ = Vector2.Distance(targetPosXZ, headPosXZ);

    //    float directionY = Target.position.y - _headTransform.position.y + offsetY;

    //    float angleZ = Mathf.Atan2(distanceXZ, directionY) * Mathf.Rad2Deg - 90f;
    //    angleZ *= -1f;

    //    _headTransform.localRotation = Quaternion.Slerp(_headTransform.localRotation, Quaternion.Euler(0f, 0f, angleZ), RotateSpeed * Time.deltaTime);
    //}

    public void FireToEnemy(Transform Target)
    {
        //Vector3 _direction = (Target.position - _firePoint.position).normalized;
        //Ray ray = new Ray(_firePoint.position, _direction);//_firepoint forward olsun, target position olmasýn
        Ray ray = new Ray(_firePoint.position, _firePoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, ShootingRange, shootingLayer))//layer sorgusu, enemy görmesin, enemyBodyPart Görsün
        {
            //ammoPooling.SpawnAmmo(hit.distance); mermi oluþturma ve yok olma süresi
            if (hit.transform.CompareTag("EnemyBodyPart"))//tag olarak EnemyBodyPart, hit.collider.gameObject.GetComponent<EnemyBodyPartDamageDetection>().GetPartDamage(_damage);
            {
                print("BulletRayHit");
                ammoPooling.SpawnAmmo(hit.distance);
                hit.collider.gameObject.GetComponent<EnemyBodyPartDamageDetection>().GetPartDamage(Damage);
            }


        }
        else //ne olur ne olmaz
        {
            ammoPooling.SpawnAmmo(50);
        }



        /*
        pooledBullets[_bulletIndex].transform.position = _firePoint.position;

        //pooledBullets[_bulletIndex].transform.LookAt(Target); 
        pooledBullets[_bulletIndex].SetActive(true);

        _bulletIndex++;

        if (_bulletIndex == BulletCount)
            _bulletIndex = 0;
        */
    }
    //Bullet düþmana çarpýnca trasnform.SetActive(false)
}
