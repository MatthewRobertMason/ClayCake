using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(AudioSource))]
public class FadeMusic : MonoBehaviour
{
    public bool fadeMusic;

    [Range(0.0f, 1.0f)]
    public float startVolume = 0.0f;

    [Range(0.0f, 1.0f)]
    public float finalVolume = 0.5f;

    [Range(0.0f, 20.0f)]
    public float fadeTime = 10.0f;

    private float timeSpent = 0.0f;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
        audioSource.volume = startVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeMusic)
        {
            if (timeSpent < fadeTime)
            {
                timeSpent += Time.deltaTime;
                float volume = Mathf.Lerp(startVolume, finalVolume, (timeSpent / fadeTime));
                audioSource.volume = volume;
            }
            else
            {
                audioSource.volume = finalVolume;
                Destroy(this);
            }
        }
    }
}
