using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblePlatform : MonoBehaviour
{
    [SerializeField] private bool isDestructibleOnContact = true; // Marcar para destruir al contacto con el jugador.
    [SerializeField] private float destructionTime = 10f; // Tiempo total para destruir la plataforma.
    [SerializeField] private float percentageToDestroyCompletly = 0.05f; // Porcentaje neceario para destuirse por completo.
    [SerializeField] private float reappearTime = 60f; // Tiempo para reaparecer despues de ser destruida.
    public float destructionPercentage = 0.85f; // Porcentaje de destruccion.
    [SerializeField] private GameObject[] platformParts;

    // Variables solo para cuando está conectada a otra plataforma temproizada.
    [Header("Temporizated platforms")]
    [SerializeField] private GameObject prevPlatform; // Plataforma previa a la actual.
    [SerializeField] private bool isTemporizated = false; // Marcar para destruir despues de que la plataforma previa cumpla un porcentaje.
    public float prevPlatDestructionPercentage = 0.85f; // Porcentaje de destrucción de la anterior plataforma, necesario para comenzar la destruccion.

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}