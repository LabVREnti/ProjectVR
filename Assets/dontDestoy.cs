using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestoy : MonoBehaviour
{
    [HideInInspector]
    public string id;

    private void Awake()
    {
        id = name + transform.position.ToString();
    }

    private void Start()
    {


        for (int i = 0; i < Object.FindObjectsOfType<dontDestoy>().Length; i++)
        {
            if (Object.FindObjectsOfType<dontDestoy>()[i] != this)
            {
                if (Object.FindObjectsOfType<dontDestoy>()[i].id == id)
                {
                    Destroy(gameObject);
                }
            }
        }
        
        DontDestroyOnLoad(gameObject);
    }
}
