using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class unloadZone : MonoBehaviour
{
    [SerializeField] string sceneToUnload;
    [SerializeField] Light directionalLight;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.UnloadSceneAsync(sceneToUnload);
            directionalLight.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
