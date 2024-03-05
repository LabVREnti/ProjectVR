using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private GameObject dungeonBGM;
    private AudioSource audioBGM;
    [SerializeField] private bool thisTriggerPlaysMusic = false;
    [SerializeField] private float fadeDuration = 2f;
    private float initialVolume;
    private float fadeStartTime;
    private bool fading;

    private void Awake()
    {
        if (dungeonBGM != null)
        {
            audioBGM = dungeonBGM.GetComponent<AudioSource>();
            initialVolume = audioBGM.volume;
        }
    }

    private void Update()
    {
        if (audioBGM != null)
        {
            if (fading)
            {
                float elapsedTime = Time.time - fadeStartTime;
                if (!thisTriggerPlaysMusic)
                {
                    float fadeRatio = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
                    audioBGM.volume = initialVolume * fadeRatio;

                    if (fadeRatio <= 0)
                    {
                        audioBGM.Stop();
                        fading = false;
                    }
                }

                else
                {
                    float fadeRatio = Mathf.Clamp01(elapsedTime / fadeDuration);
                    audioBGM.volume = initialVolume * fadeRatio;

                    if (fadeRatio >= 1)
                    {
                        if (!audioBGM.isPlaying) audioBGM.Play();
                        fading = false;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) ;
        else
        {
            StartFade();
        }
    }

    public void StartFade()
    {
        if (audioBGM != null && !fading)
        {
            fadeStartTime = Time.time;
            fading = true;
        }
    }
}