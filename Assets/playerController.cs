using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] Vector3 respawn;

    private Manager manager;
    private moveEnemy ogre;

    int duckCount;

    private void Start()
    {
        duckCount = 0;
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
          //  manager.SetRespawnObject(this.gameObject, respawn);
          //  manager.SetRespawnObject(collision.gameObject, new Vector3(1.036f, -3.41f, 0f));

        }
    }

    private void Update()
    {
        if(duckCount == 5)
        {
            Debug.Log("TODOS LOS PATITOS");
        }
    }

    public void AddDuck()
    {
        duckCount++;
    }
}
