using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyPartDamageDetection : MonoBehaviour
{
    [SerializeField] EnemyHealth enemyHealth;
    int headShotMult;
    [SerializeField] BodyParts bodyPart;
    public enum BodyParts
    {
        Head,
        arm,
        leg,
        torso
    }
    public void GetPartDamage(int _damage)//move headshot to enemyhealth
    {
        enemyHealth.GetDamage(_damage,bodyPart);
    }
}
