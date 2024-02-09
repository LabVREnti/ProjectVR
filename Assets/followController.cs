using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable] public struct controller{
    public Controller trigger;
    public bool condition;

}
public class followController : MonoBehaviour
{
    public controller[] triggers;

    moveEnemy ogre;
    public bool follow = false;

    private void Start()
    {
        ogre = FindAnyObjectByType<moveEnemy>();
    }

}

public class Controller : MonoBehaviour
{
    public bool c;
    Controller(bool condition)
    {
        c = condition;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            if (c)
            {
              //  ogre.SetFollow(c);
            }
            else
            {
              //  ogre.SetFollow(c);
            }
        }
    }

    public void GetC()
    {

    }
}
