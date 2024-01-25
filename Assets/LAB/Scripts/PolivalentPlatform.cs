using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolivalentPlatform : MonoBehaviour
{
    [Header("Platform Type")]
    [Tooltip("Final position")] [SerializeField] private bool isLavaPlatform = false;
    [Tooltip("Final position")] [SerializeField] private bool lavaPlatformActivated = false;

    [Header("Player reference")]
    [Tooltip("Player reference")][SerializeField] private CharacterController player; // Variable to moves the player with the plarform

    [Header("Movement")]
    private Vector3 oPos;
    [Tooltip("Final position")] [SerializeField] private Vector3 fPos;
    [Tooltip("Duration of displacement (origin to final)")] [SerializeField] private float dispDurationOToF = 2.0f;
    [Tooltip("Duration of displacement (final to origin)")] [SerializeField] private float dispDurationFToO = 2.0f;
    [Tooltip("Time before move (origin to final)")] [SerializeField] private float timeToStartOToF = 3.0f;
    [Tooltip("Time before move (final to origin)")] [SerializeField] private float timeToStartFToO = 3.0f;
    [Tooltip("Does the platform move from the origin to the end?")] [SerializeField] private bool movingOToF = true;
    [Tooltip("Does the platform move from the end to the origin?")] [SerializeField] private bool movingFToO = false;
    [Tooltip("Player has touched the platform?")] [SerializeField] private bool touchedByPlayer = false;
    [Tooltip("Ogre has touched the platform?")] [SerializeField] private bool touchedByOgre = false;
    private float timeToStartOToFBackup;
    private float timeToStartFToOBackup;
    private float elapsedTime = 0.0f;
    private Vector3 prevPos; // Platform previous position

    [Header("Route prediction")]
    [Tooltip("Color of the predictive route line")][SerializeField] private Color predictionColor = Color.yellow;

    // Start is called before the first frame update
    void Start()
    {
        oPos = transform.position;
        elapsedTime = 0.0f;
        timeToStartOToFBackup = timeToStartOToF;
        timeToStartFToOBackup = timeToStartFToO;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;

        #region PlatformMovement
        if (isLavaPlatform)
        {
            if (lavaPlatformActivated)
            {
                timeToStartOToF -= Time.deltaTime;
                if (timeToStartOToF <= 0.0f)
                {
                    movingOToF = true;
                    if (movingOToF)
                    {
                        if (elapsedTime < dispDurationOToF)
                        {
                            // Calculate completed percentage
                            float completed = elapsedTime / dispDurationOToF;

                            // Lineal interpolation between start and final positions
                            transform.position = Vector3.Lerp(oPos, fPos, completed);
                        }
                        else
                        {
                            // Movement finished
                            transform.position = fPos;
                            movingOToF = false;
                            elapsedTime = 0.0f;
                        }
                    }
                    else
                    {
                        timeToStartFToO -= Time.deltaTime;
                        if (timeToStartFToO <= 0.0f)
                        {
                            movingFToO = true;
                            if (movingFToO)
                            {
                                if (elapsedTime < dispDurationFToO)
                                {
                                    // Calculate completed percentage
                                    float completed = elapsedTime / dispDurationFToO;

                                    // Lineal interpolation between start and final positions
                                    transform.position = Vector3.Lerp(fPos, oPos, completed);
                                }
                                else
                                {
                                    // Movement finished
                                    transform.position = oPos;
                                    movingFToO = false;
                                    elapsedTime = 0.0f;
                                }
                            }
                        }
                    }
                }

                if(!movingOToF && !movingFToO)
                {
                    timeToStartFToO = timeToStartFToOBackup;
                    timeToStartOToF = timeToStartOToFBackup;
                    lavaPlatformActivated = false;
                }
            }
        }
        else
        {
            if (movingOToF)
            {
                if (elapsedTime < dispDurationOToF)
                {
                    // Calculate completed percentage
                    float completed = elapsedTime / dispDurationOToF;

                    // Lineal interpolation between start and final positions
                    transform.position = Vector3.Lerp(oPos, fPos, completed);
                }
                else
                {
                    // Movement finished
                    transform.position = fPos;
                    movingOToF = false;
                    elapsedTime = 0.0f;
                }
            }
            else
            {
                if (elapsedTime < dispDurationFToO)
                {
                    // Calculate completed percentage
                    float completed = elapsedTime / dispDurationFToO;

                    // Lineal interpolation between start and final positions
                    transform.position = Vector3.Lerp(fPos, oPos, completed);
                }
                else
                {
                    // Movement finished
                    transform.position = oPos;
                    movingOToF = true;
                    elapsedTime = 0.0f;
                }
            }
        }
        #endregion

        #region Collision actions
        if (touchedByPlayer)
        {
            if (player != null)
            {
                // Calcular el desplazamiento de la plataforma y la diferencia entre la posición actual y la anterior
                Vector3 displacement = transform.position - prevPos;

                // Mover al jugador con el mismo desplazamiento que la plataforma
                player.Move(displacement);
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
                timeToStartOToF = timeToStartOToFBackup;
                lavaPlatformActivated = true;
            }
        }

        if (collider.gameObject.CompareTag("Ogre"))
        {
            touchedByOgre = true;
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
}