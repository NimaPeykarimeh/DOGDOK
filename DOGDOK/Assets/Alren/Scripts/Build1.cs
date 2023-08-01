using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Build1")]
public class Build1 : ScriptableObject
{
    public string buildingName;
    public Sprite buildingImage;
    public float buildingDuration;
    public Vector3 buildingSize;
    public List<Resource1> requiredResource;
    public List<int> requiredAmount;
}
