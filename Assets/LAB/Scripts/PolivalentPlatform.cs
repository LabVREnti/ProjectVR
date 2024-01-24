using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolivalentPlatform : MonoBehaviour
{
    [Tooltip("Original position")] [SerializeField] private Vector3 oPos;
    [Tooltip("Final position")] [SerializeField] private Vector3 fPos;
    [Tooltip("Duration of displacement (origin to final)")] [SerializeField] private float dispDurationOToF = 2.0f;
    [Tooltip("Duration of displacement (final to origin)")] [SerializeField] private float dispDurationFToO = 2.0f;
    [Tooltip("Time before move (origin to final) *To be implemented*")] [SerializeField] private float timeToStartOToF = 3.0f; // To be implemented (delete commentary in tooltip when implemented)
    [Tooltip("Time before move (final to origin) *To be implemented*")] [SerializeField] private float timeToStartFToO = 3.0f; // To be implemented (delete commentary in tooltip when implemented)
    [Tooltip("Elapsed time")] [SerializeField] private float elapsedTime = 0.0f;
    [Tooltip("Does the platform move from the origin to the end?")] [SerializeField] private bool movingOToF = true;
    [Tooltip("Player has touched the platform?")] [SerializeField] private bool touchedByPlayer = false;
    [Tooltip("Ogre has touched the platform?")] [SerializeField] private bool touchedByOgre = false;

    [Header("Route prediction")]
    [Tooltip("Color of the predictive route line")][SerializeField] private Color predictionColor = Color.yellow;
    [Tooltip("Size of the predictive route line.")][SerializeField] private Vector3 lineSize = new(1f, 1f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (movingOToF){
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

    // Draw line from oPosition to fPosition
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(oPos, fPos);
    }
}