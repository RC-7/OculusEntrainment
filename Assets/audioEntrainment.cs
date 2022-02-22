using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioEntrainment : MonoBehaviour
{
    /*[Range(1, 20000)]  //Creates a slider in the inspector*/
    public float frequency1;

    /*[Range(1, 20000)]  //Creates a slider in the inspector*/
    public float frequency2;

    public float sampleRate = 44100;
    public float waveLengthInSeconds = 2.0f;

    AudioSource audioSource;
    int timeIndex = 0;

    void Start()
    {

        frequency1 = 500.0f; // Make this difference dynamic based on set value
        frequency2 = 503.0f;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;
        audioSource.spatialBlend = 0; //force 2D sound
        audioSource.Stop(); //avoids audiosource from starting to play automatically
    }

    void Update()
    {
        // Add logic to reset when we swicth frequency

        if (!audioSource.isPlaying)
        {
            timeIndex = 0;  //resets timer before playing sound
            audioSource.Play();
        }
    }

    public void UpdateAudioEntrainment(float baseFrequency, float entrainmentFrequency)
    {
        frequency1 = baseFrequency;
        frequency2 = baseFrequency + entrainmentFrequency;
        waveLengthInSeconds = 1 / baseFrequency; // Improve this, is it even needed?

        if (!audioSource.isPlaying)
        {
            timeIndex = 0;  //resets timer before playing sound
            audioSource.Play();
        }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            data[i] = CreateSine(timeIndex, frequency1, sampleRate);

            if (channels == 2)
                data[i + 1] = CreateSine(timeIndex, frequency2, sampleRate);

            timeIndex++;

            //if timeIndex gets too big, reset it to 0
            if (timeIndex >= (sampleRate * waveLengthInSeconds))
            {
                timeIndex = 0;
            }
        }
    }

    //Creates a sinewave
    public float CreateSine(int timeIndex, float frequency, float sampleRate)
    {
        return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate);
    }

}
