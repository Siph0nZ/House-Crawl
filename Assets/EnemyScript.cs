using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{   
    [SerializeField] Transform player; 
    NavMeshAgent agent;
    private float aiSpeed;
    private float distance;

    private void Start()
    {   
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false; 
    }

    void Update()
    {   
        agent.SetDestination(player.position);
    }

     /*
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, aiSpeed * Time.deltaTime);
    }
    private void Update()
    {
        player.SetDestination(target.position);
    }

    public GameObject player;
    public float aiSpeed;
    private float distance;

    void Start()
    {
        player = GameObject.FindWithTag("Player"); // finds clone with player tag
    }
    */
}
