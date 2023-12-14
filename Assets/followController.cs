using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followController : MonoBehaviour
{
    [SerializeField] moveEnemy ogre;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ogre.SetFollow(true);
        }
    }
}
