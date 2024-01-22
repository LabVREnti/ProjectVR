using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] Vector3 respawn;

    private Manager manager;
    private moveEnemy ogre;

    private void Start()
    {
        manager = FindObjectOfType<Manager>();
        ogre  = FindObjectOfType<moveEnemy>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ogre"))
        {
            // Reset ogre stateFollow
            
            ogre.SetFollow(false);

            //Respawn player and ogre
            manager.SetRespawnObject(this.gameObject, respawn);
            manager.SetRespawnObject(collision.gameObject, new Vector3(3.6f, 1.0f, -15.0f));

        }
    }
}
