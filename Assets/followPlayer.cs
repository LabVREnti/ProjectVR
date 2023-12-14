using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform enemy;
    [SerializeField] float speed = 6.0f;

    public void Follow()
    {
        enemy.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Follow();
    }
}
