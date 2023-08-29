using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    [SerializeField] float noiseRange;
    [SerializeField] Transform center;
    [SerializeField] LayerMask enemyLayer;

    [Header("Gizmo")]
    private bool showGizmos = false;
    [SerializeField] float gizmoTimer;
    [SerializeField] float gizmoDuration = 0.1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (showGizmos)
        {
            gizmoTimer += Time.deltaTime;
            if (gizmoTimer >= gizmoDuration)
            {
                showGizmos = false;
                gizmoTimer = 0;
            }
        }
    }

    public void MakeNoise(float _range,Transform _noiseCenter)
    {
        showGizmos = true;
        center = _noiseCenter;
        noiseRange = _range;
        Collider[] hitColliders = Physics.OverlapSphere(_noiseCenter.position, _range, enemyLayer);

        foreach (Collider collider in hitColliders)
        {
            // Do something with the detected enemy (e.g., damage, apply an effect, etc.)
            collider.gameObject.GetComponent<EnemyController>().AlertEnemy();
            //Debug.Log("Enemy detected: " + collider.gameObject.name);
        }
        
    }
    private void OnDrawGizmos()//To display
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;

            int numPoints = 30; // Number of points to approximate the circle
            float angleIncrement = 360f / numPoints;

            Vector3 prevPoint = center.position + Vector3.forward * noiseRange;
            for (int i = 0; i <= numPoints; i++)
            {
                float angle = i * angleIncrement;
                float x = Mathf.Sin(angle * Mathf.Deg2Rad) * noiseRange;
                float z = Mathf.Cos(angle * Mathf.Deg2Rad) * noiseRange;
                Vector3 currentPoint = center.position + new Vector3(x, 0f, z);

                if (i > 0)
                {
                    Gizmos.DrawLine(prevPoint, currentPoint);
                }

                prevPoint = currentPoint;
            }
        }
    }
}
