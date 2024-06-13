using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{   
    [SerializeField] Transform player; 
    NavMeshAgent agent;
    private float aiSpeed = 3;
    [SerializeField] private float minDistance;
    [SerializeField] private float stopDistance;

    private void Start()
    {   
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false; 
    }

    void Update()
    {   
        // follows player based off distance
        if (Vector3.Distance(player.position, transform.position) < minDistance)
        {   
            GetComponent<NavMeshAgent>().speed = aiSpeed;
            agent.SetDestination(player.position);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Light")
        {
            aiSpeed = 0;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        aiSpeed = 3;
    }
}
