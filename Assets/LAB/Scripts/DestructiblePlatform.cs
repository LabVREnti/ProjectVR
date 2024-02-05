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
    [Tooltip("Platform destruction is active?")][SerializeField] private bool destroying = false;
    [SerializeField] private DestructionState destructionState = DestructionState.NONE; // Estado de la destruccion de la plataforma.
    [SerializeField] private float destructionTime = 10f; // Tiempo total para destruir la plataforma.
    [SerializeField] private float percentageToDestroyCompletly = 0.05f; // Porcentaje neceario para destuirse por completo.
    public float destructionPercentage = 0.0f; // Porcentaje de destruccion.

    [SerializeField] private List<Transform> platformParts;
    private List<Vector3> platformPartsOriginalPos;
    [SerializeField] private List<Transform> platPartsDestroyed;
    [Tooltip("Gravity of the platform parts when has destroyed")][SerializeField] private Vector3 fakeGravity = new Vector3(0, -4, 0);
    [Tooltip("Time since the platform part has destroyed for calculate the delay for destroy next part")]
    [SerializeField] private float destructionElapsedTime = 0.0f;
    [Tooltip("Delay between destroy platform parts")][SerializeField] private float destroyDelay = 0.5f;

    // Variables solo para cuando est� conectada a otra plataforma temproizada.
    [Header("Temporizated platforms")]
    [SerializeField] private GameObject prevPlatform; // Plataforma previa a la actual.
    [SerializeField] private bool isTemporizated = false; // Marcar para destruir despues de que la plataforma previa cumpla un porcentaje.
    public float prevPlatDestructionPercentage = 0.85f; // Porcentaje de destrucci�n de la anterior plataforma, necesario para comenzar la destruccion.

    // Start is called before the first frame update
    void Start()
    {
        if (player == null) player = FindObjectOfType<playerController>();
        if (ogre == null) ogre = FindObjectOfType<moveEnemy>();

        oPos = transform.position;
        elapsedTime = 0.0f;
        platformState = PlatformState.WAITINGONORIGIN;

        destructionState = DestructionState.NONE;

        for (int i = 0; i < transform.childCount; i++)
        {
            platformParts.Add(transform.GetChild(i));
        }

        foreach (Transform objetoTransform in platformParts)
        {
            // POR ALGUN MOTIVO ESTA PARTE NO FUNCIONA. ARREGLAR ESTO.
            platformPartsOriginalPos.Add(objetoTransform.position);
        }

        destructionElapsedTime = destroyDelay;
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
                    // Calcular el desplazamiento de la plataforma y la diferencia entre la posici�n actual y la anterior
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
                    // Calcular el desplazamiento de la plataforma y la diferencia entre la posici�n actual y la anterior
                    Vector3 displacement = transform.position - prevPos;

                    // Mover al jugador con el mismo desplazamiento que la plataforma
                    ogre.GetComponent<Rigidbody>().AddForce(displacement);
                }
            }
        }

        if ((touchedByPlayer || touchedByOgre) && destructionState == DestructionState.NONE)
        {
            destroying = true;
        }

        if (destroying)
        {
            DestroyPlatform();
        }

        // Actualizar la posici�n anterior de la plataforma con la posici�n actual
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

    private void PlatformMovement()
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

    private void SlidingMovement(Vector3 origin, Vector3 destiny, float timeToDestiny)
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

    private void PlatformWaitingToBeTouched(ref float timeToWait)
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

    private void DestroyPlatform()
    {
        switch (destructionState)
        {
            case DestructionState.NONE:
                destructionState = DestructionState.FASE1;
                break;

            case DestructionState.FASE1:
                destruction(2);
                destructionState = DestructionState.FASE2;
                break;

            case DestructionState.FASE2:
                destruction(1);
                destructionState = DestructionState.FASE3;
                break;

            case DestructionState.FASE3:
                destruction(0);
                destructionState = DestructionState.DESTROYED;
                break;

            case DestructionState.DESTROYED:
                for (int i = 0; i < platformPartsOriginalPos.Count; i++)
                {
                    platformParts[i].position = platformPartsOriginalPos[i];
                }
                platPartsDestroyed.Clear();
                destroying = false;
                destructionState = DestructionState.NONE;
                break;
        }
    }

    private void destruction(int remainingParts)
    {
        if (platformParts != null && platformParts.Count > 0)
        {
            int i = platformParts.Count / remainingParts + 1;
            destructionElapsedTime += Time.fixedDeltaTime;
            while (i >= 0)
            {
                if (destructionElapsedTime >= destroyDelay)
                {
                    destroyAndReturnPart().GetComponent<Rigidbody>().velocity += fakeGravity * Time.fixedDeltaTime;
                    destructionElapsedTime = 0.0f;
                    i--;
                }
            }
        }
    }

    private Transform destroyAndReturnPart()
    {
        // Obt�n un �ndice aleatorio dentro del rango del array
        int aleatoryPart = Random.Range(0, platformParts.Count);

        // Accede al Transform correspondiente al �ndice aleatorio
        Transform platformPart = platformParts[aleatoryPart];

        // A�adimos la parte al listado de partes destru�das y la eliminamos del que contiene las intactas
        platPartsDestroyed.Add(platformPart);
        platformParts.RemoveAt(aleatoryPart);

        return platformPart;
    }
}