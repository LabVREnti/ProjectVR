using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class moveEnemy : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;

    [SerializeField] bool followAlways;
    bool follow;
    bool stunned;

    float timer = 3.0f;

    Animator anim;
    Rigidbody rb;
    private void Start()
    {
        anim = GetComponent<Animator>();
        player = FindAnyObjectByType<playerController>().GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        follow = false;
        stunned = false;
    }
    void Update()
    {
        if(stunned)
        {
            follow = false;
            rb.velocity = Vector3.zero;
            timer -= Time.deltaTime;
            anim.SetBool("Die", true);

            if (timer <= 0.0f) { 
                follow = true;
                timer = 3.0f;
                stunned = false;
                anim.SetBool("Die", false);
                anim.SetBool("Walk", true);
            }
            Debug.Log("stunneado");
        }
        else { Debug.Log("vuelta al follow"); }

        if (follow)
        {
            agent.SetDestination(player.transform.position);
            anim.SetBool("Walk", true);
        }

        if (followAlways)
        {    
            agent.SetDestination(player.transform.position);
            anim.SetBool("Walk", true);       
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            follow = true;
            //setfollow true huevona
            anim.SetBool("Walk", true);
        } 
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            follow = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Torch"))
        {
            // Play sonido de hacer da�o al ogro y animacion da�o ogro
            stunned = true;
            Debug.Log("te he pegado");
        }

    }

    public void SetFollow(bool condition)
    {
        this.follow = condition;
    }
}
