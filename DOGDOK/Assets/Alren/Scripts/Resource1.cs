using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
[CreateAssetMenu(menuName ="Resource1")]
public class Resource1 : ScriptableObject
{
    [HideInInspector] public int id;
    public string resourceName;
    public Sprite resourceImage;
    public Mesh resourceMesh;
}
