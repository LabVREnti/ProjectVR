using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class teleportToNexus : MonoBehaviour
{
    public string sceneName = "Nexus"; // Nombre de la escena a la que quieres cambiar
    public float secondsToWait = 3f; // Tiempo de espera para cambiar de escena
    public Vector3 spawnPosition = new Vector3(-0.496f, 1.45f, -0.21f); // La posición de aparición en la nueva escena
    public Quaternion spawnRotation = new Quaternion(); // La rotación en la nueva escena

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Inicia la coroutine para esperar antes de cambiar de escena
            StartCoroutine(WaitAndLoadScene());
        }
    }

    // Coroutine que espera un número de segundos antes de cargar la escena
    IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(secondsToWait); // Espera 3 segundos
        SceneManager.LoadScene(sceneName); // Carga la escena
        SceneManager.sceneLoaded += OnSceneLoaded; // Suscribe el método al evento de carga de escena
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == sceneName)
        {
            // Encuentra al jugador en la nueva escena
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            // Mueve al jugador a la posición de aparición
            player.transform.position = spawnPosition;
            player.transform.rotation = spawnRotation;
            // Desuscribe el método para evitar que se llame múltiples veces
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
