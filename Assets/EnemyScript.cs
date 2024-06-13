using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{   
    [SerializeField] Transform player; 
    NavMeshAgent agent;
    private float aiSpeed = 3;
    [SerializeField] private float minDistance;
    [SerializeField] private float stopDistance;
    FadeInOut fade;
    AudioSource audioSource;
    bool isFound = false;


    private void Start()
    {   
        fade = FindObjectOfType<FadeInOut>();
        audioSource = GetComponent<AudioSource>();
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

    void OnCollisionEnter2D(Collision2D other)
    {   
        if (other.gameObject.CompareTag("Player"))
        {   
            StartCoroutine(DeathScene());
        }
    }

    public IEnumerator DeathScene()
    {
        fade.FadeIn();
        new WaitForSeconds(1);
        SceneManager.LoadScene("Death Screen");
        yield return new WaitForSeconds(1);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Light")
        {
            aiSpeed = 0;

            if (!isFound)
            {
                audioSource.Play();
                isFound = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        aiSpeed = 3;
        audioSource.Stop();
    }
}
