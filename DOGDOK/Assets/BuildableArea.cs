using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableArea : MonoBehaviour
{
    private Vector2 positionArea1; // Küp'ün deðerleri büyük köþesi
    private Vector2 positionArea2; // Küp'ün deðerleri küçük köþesi

    void Start()
    {
        positionArea1 = new Vector2(gameObject.transform.position.x + gameObject.transform.localScale.x / 2, gameObject.transform.position.z + gameObject.transform.localScale.z / 2);
        positionArea2 = new Vector2(gameObject.transform.position.x - gameObject.transform.localScale.x / 2, gameObject.transform.position.z - gameObject.transform.localScale.z / 2);
    }

    public bool CheckBuildableArea(Vector3 positionTurret)
    {
        if (
            positionTurret.x > positionArea2.x && positionTurret.z > positionArea2.y
                                               &&
            positionTurret.x < positionArea1.x && positionTurret.z < positionArea1.y
           )
            return true;

        return false;
    }

}
