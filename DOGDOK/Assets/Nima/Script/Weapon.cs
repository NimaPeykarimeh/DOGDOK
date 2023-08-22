using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Build")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public Sprite weaponImage;
    public WeaponController.WeaponType weaponType;
}
