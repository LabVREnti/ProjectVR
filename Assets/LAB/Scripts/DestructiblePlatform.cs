using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestructiblePlatform : MonoBehaviour
{
    private enum PlatformState
    {
        TOFINAL,
        TOORIGIN,
        WAITINGONORIGIN,
        WAITINGONFINAL
    }

    private enum DestructionState
    {
        NONE,
        FASE1,
        FASE2,
        FASE3,
        DESTROYED
    }

    [Header("Platform Type")]
    [Tooltip("Final position")][SerializeField] private bool isLavaPlatform = false;
    [Tooltip("Final position")][SerializeField] private bool lavaPlatformActivated = false;

    //[Header("Player and Ogre reference")]
    /*[Tooltip("Player reference")][SerializeField]*/
    private playerController player; // Variable to moves the player with the plarform
    /*[Tooltip("Ogre reference")][SerializeField]*/
    private moveEnemy ogre; // Variable to moves the player with the plarform

    [Header("Movement")]
    private Vector3 oPos;
    [Tooltip("Final position")][SerializeField] private Vector3 fPos;
    [Tooltip("Duration of displacement (origin to final)")][SerializeField] private float dispDurationToFinal = 2.0f;
    [Tooltip("Duration of displacement (final to origin)")][SerializeField] private float dispDurationToOrigin = 2.0f;
    [Tooltip("Time before move (origin to final)")][SerializeField] private float waitTimeToMoveToFinal = 3.0f;
    [Tooltip("Time before move (final to origin)")][SerializeField] private float waitTimeToMoveToOrigin = 3.0f;
    [Tooltip("Platform state")][SerializeField] private PlatformState platformState;
    /*[Tooltip("Player has touched the platform?")] [SerializeField]*/
    private bool touchedByPlayer = false;
    /*[Tooltip("Ogre has touched the platform?")] [SerializeField]*/
    private bool touchedByOgre = false;
    private float elapsedTime = 0.0f;
    private Vector3 prevPos; // Platform previous position

    [Header("Route prediction")]
    [Tooltip("Color of the predictive route line")][SerializeField] private Color predictionColor = Color.yellow;

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
        if (player == null) player = FindObjectOfType<playerController>();
        if (ogre == null) ogre = FindObjectOfType<moveEnemy>();

        oPos = transform.position;
        elapsedTime = 0.0f;
        platformState = PlatformState.WAITINGONORIGIN;

        destructionState = DestructionState.NONE;

        platformParts = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            platformParts[i] = transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsedTime += Time.fixedDeltaTime;

        PlatformMovement();

        #region Collision actions
        if (touchedByPlayer)
        {
            if (player != null)
            {
                if (platformState == PlatformState.TOFINAL || platformState == PlatformState.TOORIGIN)
                {
                    // Calcular el desplazamiento de la plataforma y la diferencia entre la posición actual y la anterior
                    Vector3 displacement = transform.position - prevPos;

                    // Mover al jugador con el mismo desplazamiento que la plataforma
                    player.GetComponent<Rigidbody>().AddForce(displacement);
                }
            }
        }

        if (touchedByOgre)
        {
            if (ogre != null)
            {
                if (platformState == PlatformState.TOFINAL || platformState == PlatformState.TOORIGIN)
                {
                    // Calcular el desplazamiento de la plataforma y la diferencia entre la posición actual y la anterior
                    Vector3 displacement = transform.position - prevPos;

                    // Mover al jugador con el mismo desplazamiento que la plataforma
                    ogre.GetComponent<Rigidbody>().AddForce(displacement);
                }
            }
        }

        // Actualizar la posición anterior de la plataforma con la posición actual
        prevPos = transform.position;
        #endregion
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            touchedByPlayer = true;
            if (isLavaPlatform && !lavaPlatformActivated)
            {
                lavaPlatformActivated = true;
            }
        }

        if (collider.gameObject.CompareTag("Ogre"))
        {
            touchedByOgre = true;
            if (isLavaPlatform && !lavaPlatformActivated)
            {
                lavaPlatformActivated = true;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            touchedByPlayer = false;
        }

        if (collider.gameObject.CompareTag("Ogre"))
        {
            touchedByOgre = false;
        }
    }

    // Draw line from oPosition to fPosition
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, fPos);
    }

    void PlatformMovement()
    {
        switch (platformState)
        {
            case PlatformState.TOFINAL:
                SlidingMovement(oPos, fPos, dispDurationToFinal);
                break;

            case PlatformState.TOORIGIN:
                SlidingMovement(fPos, oPos, dispDurationToOrigin);
                break;

            case PlatformState.WAITINGONORIGIN:
                if (!isLavaPlatform) { platformState = PlatformState.TOFINAL; }
                else
                {
                    PlatformWaitingToBeTouched(ref waitTimeToMoveToFinal);
                }
                break;

            case PlatformState.WAITINGONFINAL:
                if (!isLavaPlatform) { platformState = PlatformState.TOORIGIN; }
                else
                {
                    PlatformWaitingToBeTouched(ref waitTimeToMoveToOrigin);
                }
                break;
        }
    }

    void SlidingMovement(Vector3 origin, Vector3 destiny, float timeToDestiny)
    {
        if (elapsedTime < timeToDestiny)
        {
            // Calculate completed percentage
            float completed = elapsedTime / timeToDestiny;

            // Lineal interpolation between start and final positions
            transform.position = Vector3.Lerp(origin, destiny, completed);
        }
        else
        {
            // Movement finished
            transform.position = destiny;
            elapsedTime = 0.0f;

            if (platformState == PlatformState.TOFINAL)
            {
                if (!isLavaPlatform) platformState = PlatformState.TOORIGIN;
                else platformState = PlatformState.WAITINGONFINAL;
            }
            else if (platformState == PlatformState.TOORIGIN)
            {
                if (!isLavaPlatform) platformState = PlatformState.TOFINAL;
                else
                {
                    platformState = PlatformState.WAITINGONORIGIN;
                    lavaPlatformActivated = false;
                }
            }
        }
    }

    void PlatformWaitingToBeTouched(ref float timeToWait)
    {
        float timeBackup = timeToWait;
        if (lavaPlatformActivated)
        {
            timeToWait -= Time.fixedDeltaTime;
            if (timeToWait <= 0.0f)
            {
                elapsedTime = 0.0f;
                if (platformState == PlatformState.WAITINGONORIGIN)
                {
                    timeToWait = timeBackup;
                    platformState = PlatformState.TOFINAL;
                }
                else if (platformState == PlatformState.WAITINGONFINAL)
                {
                    timeToWait = timeBackup;
                    platformState = PlatformState.TOORIGIN;
                }
            }
        }
    }
}