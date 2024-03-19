using Autohand;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] Vector3 respawn;

    private moveEnemy ogre;

    //********** MOVIMIENTO PLATAFORMA
    [SerializeField] bool omniMovement;
    OmniMovementComponent omni;
    CharacterController cc;

    //********** MOVIMIENTO MANDO
    // [SerializeField] Hand leftHand;

    string key;

    private void Start()
    {
        omni = GetComponent<OmniMovementComponent>();
        cc = GetComponent<CharacterController>();   
        ogre  = FindObjectOfType<moveEnemy>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ogre"))
        {
            

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

        Debug.Log(omni.couplingPercentage);

    }

    public void AddKey(int keyNum, bool updateTo)
    {
      //  KeyManager.Instance.UpdateKeys(keyNum, updateTo);
    }

    void UseOmniInputToMovePlayer()
    {
        if (omni.omniFound)
        {
            omni.GetOmniInputForCharacterMovement();
            omni.couplingPercentage = 0f;
        }
        else if (omni.developerMode)
            omni.DeveloperModeUpdate();


        if (omni.GetForwardMovement() != Vector3.zero)
            cc.Move(omni.GetForwardMovement());
        if (omni.GetStrafeMovement() != Vector3.zero)
            cc.Move(omni.GetStrafeMovement());
    }

}
