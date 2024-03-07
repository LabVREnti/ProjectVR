using System.Collections;
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
            if (audioBGM != null)
            {
                initialVolume = audioBGM.volume;
            }
        }
    }

    private void Update()
    {
        if (audioBGM != null && fading)
        {
            float elapsedTime = Time.time - fadeStartTime;
            float fadeRatio = thisTriggerPlaysMusic ? elapsedTime / fadeDuration : 1 - (elapsedTime / fadeDuration);
            audioBGM.volume = Mathf.Lerp(0, initialVolume, fadeRatio);

            if (thisTriggerPlaysMusic && !audioBGM.isPlaying) audioBGM.Play();

            if (fadeRatio is >= 1 or <= 0)
            {
                if (!thisTriggerPlaysMusic)
                {
                    audioBGM.Stop();
                }
                fading = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartFade();
        }
    }

    private void StartFade()
    {
        if (audioBGM != null && !fading)
        {
            fadeStartTime = Time.time;
            fading = true;
        }
    }
}