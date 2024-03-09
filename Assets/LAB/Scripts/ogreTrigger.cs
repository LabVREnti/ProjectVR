using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ogreTrigger : MonoBehaviour
{
    public bool follow;
    moveEnemy ogre;

    private void Start()
    {
        ogre = FindAnyObjectByType<moveEnemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ogre.SetFollow(follow);
        }
    }
}
