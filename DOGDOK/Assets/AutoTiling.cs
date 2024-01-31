using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTiling : MonoBehaviour
{

    Material _material;
    [SerializeField] float tileMultiply = 0.5f;
    [SerializeField] Vector2 scale;
    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<Renderer>().material;
        scale = new Vector2(transform.localScale.x, transform.localScale.y);
        _material.SetVector("_Tiling", scale * tileMultiply);
    }
}
