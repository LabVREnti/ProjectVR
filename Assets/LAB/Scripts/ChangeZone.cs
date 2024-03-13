using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeZone : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UnityEngine.SceneManagement.Scene scene = SceneManager.GetSceneByName(sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
           // Destroy(gameObject);
        }
    }
}
