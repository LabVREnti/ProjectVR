using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningObject : MonoBehaviour
{
    [SerializeField] GameObject fire;
    float timer = 0;
    private void OnParticleCollision(GameObject other)
    {
       fire.SetActive(true);

    }

    private void Update()
    {
        if (fire.activeInHierarchy)
        {
            timer += Time.deltaTime;
            if(timer > 3)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

}
