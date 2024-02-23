using Autohand;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] Vector3 respawn;

    private Manager manager;
    private moveEnemy ogre;

    //********** MOVIMIENTO PLATAFORMA
    [SerializeField] bool omniMovement;
    OmniMovementComponent omni;
    CharacterController cc;
    //********** MOVIMIENTO MANDO
   // [SerializeField] Hand leftHand;

    int duckCount;

    private void Start()
    {
        duckCount = 0;
        omni = GetComponent<OmniMovementComponent>();
        cc = GetComponent<CharacterController>();   
        manager = FindObjectOfType<Manager>();
        ogre  = FindObjectOfType<moveEnemy>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ogre"))
        {
            // Reset ogre stateFollow
            
          //  ogre.SetFollow(false);

            //Respawn player and ogre
          //  manager.SetRespawnObject(this.gameObject, respawn);
          //  manager.SetRespawnObject(collision.gameObject, new Vector3(1.036f, -3.41f, 0f));

        }
    }

    private void Update()
    {
        // Movimiento plataforma
        if (omniMovement)
        {
            UseOmniInputToMovePlayer();
        }

       // leftHand.SetEnableMovement(false);

        if(duckCount == 5)
        {
            Debug.Log("TODOS LOS PATITOS");
        }
    }

    void UseOmniInputToMovePlayer()
    {
        if (omni.omniFound)
            omni.GetOmniInputForCharacterMovement();
        else if (omni.developerMode)
            omni.DeveloperModeUpdate();


        if (omni.GetForwardMovement() != Vector3.zero)
            cc.Move(omni.GetForwardMovement());
        if (omni.GetStrafeMovement() != Vector3.zero)
            cc.Move(omni.GetStrafeMovement());
    }

    public void AddDuck()
    {
        duckCount++;
    }
}
