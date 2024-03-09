using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lava_stone_sound : MonoBehaviour
{
    [SerializeField] private List<AudioSource> stoneSources; // Lista de Audio Sources

    [Header("Time")]
    [SerializeField] private float minWaitBetweenPlays = 1f; // Tiempo mínimo de espera
    [SerializeField] private float maxWaitBetweenPlays = 5f; // Tiempo máximo de espera
    [SerializeField] private float waitTimeCountdown = -1f; // Contador de tiempo de espera

    [Header("Pitch")]
    [SerializeField] private float minPitch = 0.9f; // Variación del Pitch máxima en descenso
    [SerializeField] private float maxPitch = 1.1f; // Variación del Pitch máxima en aumento

    [Header("Volume")]
    [SerializeField] private float minVol = 0.7f; // Volumen mínimo
    [SerializeField] private float maxVol = 1f; // Volumen máximo


    private void Update()
    {
        // Verifica si hay menos de 3 clips reproduciéndose
        int playingCount = 0;
        foreach (var source in stoneSources)
        {
            if (source.isPlaying)
                playingCount++;
        }

        if (playingCount < 3)
        {
            // Si el tiempo de espera ha transcurrido, elige un nuevo Audio Source y comienza a reproducirlo
            if (waitTimeCountdown < 0f)
            {
                PlayRandomSource();
                waitTimeCountdown = Random.Range(minWaitBetweenPlays, maxWaitBetweenPlays);
            }
            else
            {
                waitTimeCountdown -= Time.deltaTime;
            }
        }
    }

    private void PlayRandomSource()
    {
        int randomIndex = Random.Range(0, stoneSources.Count);
        AudioSource randomSource = stoneSources[randomIndex];

        // Variación aleatoria del pitch (entre 0.9 y 1.1)
        float randomPitch = Random.Range(minPitch, maxPitch);
        randomSource.pitch = randomPitch;

        // Variación aleatoria del volumen (entre 0.7 y 1.0)
        float randomVolume = Random.Range(minVol, maxVol);
        randomSource.volume = randomVolume;

        randomSource.Play();
    }
}