using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTest : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] float distance;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            navMeshAgent.destination = targetTransform.position;
            distance = navMeshAgent.remainingDistance;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            NavMeshPath _path = new NavMeshPath();
            navMeshAgent.CalculatePath(targetTransform.position,_path);
            distance = 0;
            for (int i = 1; i < _path.corners.Length; i++)
            {
                distance += Vector3.Distance(_path.corners[i - 1], _path.corners[i]);
            }
        }
            
    }
}
