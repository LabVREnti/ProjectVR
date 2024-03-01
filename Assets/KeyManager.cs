using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyManager : MonoBehaviour 
{
    bool[] keys = new bool[2];

    public static KeyManager Instance;

    private void Awake()
    {
        CreateSingleton();
    }

    void CreateSingleton()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void UpdateKeys(int keyToUpdate)
    {
           switch (keyToUpdate)
            {
            case 0:
                keys[0] = true;
                break;
            case 1:
                keys[1] = true;
                break;
            case 2:
                keys[2] = true;
                break;
            }
    }


}
