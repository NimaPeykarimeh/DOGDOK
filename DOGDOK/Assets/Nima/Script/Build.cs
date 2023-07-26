using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Build")]
public class Build : ScriptableObject
{
    public string buildingName;
    public Sprite buildingImage;
    public float buildingDuration;
    public List<Resource> requiredResource;
    public List<int> requiredAmount;
}
