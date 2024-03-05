using System.Collections;
using System.Collections.Generic;
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
        DESTROYING,
        DESTROYED
    }

    [Header("Platform Type")]
    [SerializeField] private bool isLavaPlatform = false;
    [SerializeField] private bool lavaPlatformActivated = false;

    private playerController player;
    private moveEnemy ogre;

    [Header("Movement")]
    private Vector3 oPos;
    [SerializeField] private Vector3 fPosLocal = new Vector3(0.0f, -8.0f, 0.0f);
    [SerializeField] private float dispDurationToFinal = 2.0f;
    [SerializeField] private float dispDurationToOrigin = 2.0f;
    [SerializeField] private float waitTimeToMoveToFinal = 3.0f;
    [SerializeField] private float waitTimeToMoveToOrigin = 3.0f;
    private float originalWaitTimeToMoveToFinal;
    private float originalWaitTimeToMoveToOrigin;
    [SerializeField] private PlatformState platformState;
    private bool touchedByPlayer = false;
    private bool touchedByOgre = false;
    private float elapsedTime = 0.0f;
    private Vector3 prevPos;
    private AudioSource audioSource;

    [Header("Route prediction")]
    [SerializeField] private Color predictionColor = Color.yellow;

    [Header("Destructible platform variables")]
    [SerializeField] private bool destroying = false;
    [SerializeField] private DestructionState destructionState = DestructionState.NONE;
    [SerializeField] private float destructionTime = 10f;
    [SerializeField] private float percentageToDestroyCompletly = 0.05f;
    public float destructionPercentage = 0.0f;

    [SerializeField] private List<Transform> platformParts;
    private List<Vector3> platformPartsOriginalPos;
    [SerializeField] private List<Transform> platPartsDestroyed;
    [SerializeField] private Vector3 fakeGravity = new Vector3(0, -4, 0);
    [SerializeField] private float destructionElapsedTime = 0.0f;
    [SerializeField] private float destroyDelay = 0.5f;

    [Header("Temporizated platforms")]
    [SerializeField] private GameObject prevPlatform;
    [SerializeField] private bool isTemporizated = false;
    public float prevPlatDestructionPercentage = 0.85f;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GetPlayerAndOgre();
        InitPlatform();
    }

    void FixedUpdate()
    {
        elapsedTime += Time.fixedDeltaTime;
        destructionElapsedTime += Time.fixedDeltaTime;

        PlatformMovement();
        OnCollisionActions();
        DestroyPlatform();

        prevPos = transform.position;
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.TransformPoint(fPosLocal));
    }

    private void GetPlayerAndOgre()
    {
        if (player == null) player = FindObjectOfType<playerController>();
        if (ogre == null) ogre = FindObjectOfType<moveEnemy>();
    }

    private void InitPlatform()
    {
        oPos = transform.position;
        elapsedTime = 0.0f;
        platformState = PlatformState.WAITINGONORIGIN;
        destructionState = DestructionState.NONE;
        
        // Almacenar los valores originales de los tiempos de espera
        originalWaitTimeToMoveToFinal = waitTimeToMoveToFinal;
        originalWaitTimeToMoveToOrigin = waitTimeToMoveToOrigin;

        platformParts.Clear();
        platformPartsOriginalPos = new List<Vector3>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            platformParts.Add(child);
            platformPartsOriginalPos.Add(child.position);
        }
    }

    private void PlatformMovement()
    {
        switch (platformState)
        {
            case PlatformState.TOFINAL:
                SlidingMovement(oPos, transform.TransformPoint(fPosLocal), dispDurationToFinal); // Convertir fPosLocal a coordenadas mundiales
                if (audioSource != null && audioSource != audioSource.isPlaying) audioSource.Play();
                break;

            case PlatformState.TOORIGIN:
                SlidingMovement(transform.TransformPoint(fPosLocal), oPos, dispDurationToOrigin); // Convertir fPosLocal a coordenadas mundiales
                destructionState = DestructionState.DESTROYED;
                break;

            case PlatformState.WAITINGONORIGIN:
                if (!isLavaPlatform) { platformState = PlatformState.TOFINAL; }
                else { 
                    PlatformWaitingToBeMoved(ref waitTimeToMoveToFinal);
                    if (!destroying) { destructionState = DestructionState.NONE; }
                }
                break;

            case PlatformState.WAITINGONFINAL:
                if (!isLavaPlatform) { platformState = PlatformState.TOORIGIN; }
                else { PlatformWaitingToBeMoved(ref waitTimeToMoveToOrigin); }
                break;
        }
    }

    private void SlidingMovement(Vector3 origin, Vector3 destiny, float timeToDestiny)
    {
        if (elapsedTime < timeToDestiny)
        {
            float completed = elapsedTime / timeToDestiny;
            transform.position = Vector3.Lerp(origin, destiny, completed);
        }
        else
        {
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
                else { platformState = PlatformState.WAITINGONORIGIN; lavaPlatformActivated = false; }
            }
        }
    }

    private void PlatformWaitingToBeMoved(ref float timeToWait)
    {
        float timeBackup = timeToWait;
        if (lavaPlatformActivated)
        {
            timeToWait -= Time.fixedDeltaTime;
            if (timeToWait < 0.0f)
            {
                elapsedTime = 0.0f;
                if (platformState == PlatformState.WAITINGONORIGIN) platformState = PlatformState.TOFINAL;
                else if (platformState == PlatformState.WAITINGONFINAL) platformState = PlatformState.TOORIGIN;
                timeToWait = timeBackup;
            }
        }
    }

    private void OnCollisionActions()
    {
        Vector3 displacement = Vector3.zero;
        if (touchedByPlayer || touchedByOgre)
        {
            displacement = transform.position - prevPos;
        }

        if (touchedByPlayer)
        {
            if (player != null)
            {
                if (platformState is PlatformState.TOFINAL or PlatformState.TOORIGIN)
                {
                    player.GetComponent<Rigidbody>().AddForce(displacement);
                }
            }
        }

        if (touchedByOgre)
        {
            if (ogre != null)
            {
                if (platformState is PlatformState.TOFINAL or PlatformState.TOORIGIN)
                {
                    ogre.GetComponent<Rigidbody>().AddForce(displacement);
                }
            }
        }

        if ((touchedByPlayer || touchedByOgre) && destructionState == DestructionState.NONE)
        {
            destructionState = DestructionState.DESTROYING;
            destroying = true;
        }
    }

    private void DestroyPlatform()
    {
        if (destroying)
        {
            switch (destructionState)
            {
                case DestructionState.NONE:
                    break;

                case DestructionState.DESTROYING:
                    Destruction();
                    break;

                case DestructionState.DESTROYED:
                    if (platformParts.Count == 0) // Asegurarse de que todas las partes se hayan destruido antes de recuperar la plataforma
                    {
                        RecoverPlatform();
                        destroying = false;
                    }
                    break;
            }
        }
    }

    private void Destruction()
    {
        if (platformParts.Count > 0)
        {
            DropPartInTime(destroyDelay);
        }
    }

    private void DropPartInTime(float delay)
    {
        if (destructionElapsedTime >= delay)
        {
            Transform platformPart = DropAndReturnPart();
            platformPart.GetComponent<Rigidbody>().velocity += fakeGravity * Time.fixedDeltaTime;
            destructionElapsedTime = 0.0f;
        }
    }

    private Transform DropAndReturnPart()
    {
        int aleatoryPart = Random.Range(0, platformParts.Count);
        Transform platformPart = platformParts[aleatoryPart];
        platPartsDestroyed.Add(platformPart);
        platformParts.RemoveAt(aleatoryPart);

        return platformPart;
    }

    private void RecoverPlatform()
    {
        // Restaurar la posici?n original de la plataforma
        transform.position = oPos;

        // Restablecer los estados y tiempos de espera
        elapsedTime = 0.0f;
        destructionElapsedTime = 0.0f;
        platformState = PlatformState.WAITINGONORIGIN;
        lavaPlatformActivated = false;

        // Restaurar los tiempos de espera originales
        waitTimeToMoveToFinal = originalWaitTimeToMoveToFinal;
        waitTimeToMoveToOrigin = originalWaitTimeToMoveToOrigin;

        // Limpiar las partes destruidas
        platformParts.Clear();
        platPartsDestroyed.Clear();

        // Agregar de nuevo todas las partes de la plataforma
        for (int i = 0; i < transform.childCount; i++)
        {
            platformParts.Add(transform.GetChild(i));
            platformParts[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
            platformParts[i].transform.position = platformPartsOriginalPos[i];
        }
    }
}