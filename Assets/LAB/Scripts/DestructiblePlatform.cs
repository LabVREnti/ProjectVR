using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestructiblePlatform : PolivalentPlatform
{
    private enum DestructionState
    {
        NONE,
        FASE1,
        FASE2,
        FASE3,
        DESTROYED
    }

    [Header("Destructible platform variables")]
    [SerializeField] private DestructionState destructionState = DestructionState.NONE; // Estado de la destruccion de la plataforma.
    [SerializeField] private float destructionTime = 10f; // Tiempo total para destruir la plataforma.
    [SerializeField] private float percentageToDestroyCompletly = 0.05f; // Porcentaje neceario para destuirse por completo.
    public float destructionPercentage = 0.0f; // Porcentaje de destruccion.

    [SerializeField] private Transform[] platformParts;

    // Variables solo para cuando está conectada a otra plataforma temproizada.
    [Header("Temporizated platforms")]
    [SerializeField] private GameObject prevPlatform; // Plataforma previa a la actual.
    [SerializeField] private bool isTemporizated = false; // Marcar para destruir despues de que la plataforma previa cumpla un porcentaje.
    public float prevPlatDestructionPercentage = 0.85f; // Porcentaje de destrucción de la anterior plataforma, necesario para comenzar la destruccion.

    // Start is called before the first frame update
    void Start()
    {
        destructionState = DestructionState.NONE;

        platformParts = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            platformParts[i] = transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
           
    }
}