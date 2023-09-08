using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyPartDamageDetection : MonoBehaviour
{
    [SerializeField] EnemyHealth enemyHealth;
    [SerializeField] byte headShotMult = 1;
    [SerializeField] BodyParts bodyPart;
    public enum BodyParts
    {
        Head,
        arm,
        leg,
        torso
    }
    public void GetPartDamage(int _damage, EnemyHealth.HitSource _hitSource)//move headshot to enemyhealth
    {
        enemyHealth.GetDamage(_damage * headShotMult, _hitSource);
    }
}
