using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class audioEntrainment : MonoBehaviour
{
    public float frequency1;
    public float frequency2;
    public float sampleRate = 10000;
    AudioSource audioSource;
    int timeIndex = 0;

    void Start()
    {
        // Defaults to Alphda waves
        frequency1 = 450.0f;
        frequency2 = 460.0f;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;
        audioSource.spatialBlend = 0; //force 2D sound
        audioSource.Stop(); //avoids audiosource from starting to play automatically
    }

    void SaveArrayAsCSV<T>(T[] arrayToSave, string fileName)
    {
        using (StreamWriter file = new StreamWriter(fileName, true))
        {
            for (int j = 0; j<arrayToSave.Length; j += 2)
            {
                file.Write(arrayToSave[j].ToString().Replace(',', '.') + "," + arrayToSave[j+1].ToString().Replace(',', '.') + '\n');
            }
        }
    }

    void Update()
    {

        if (!audioSource.isPlaying)
        {
            Debug.Log("Resetting");
            audioSource.Play();
        }
    }

    public void UpdateAudioEntrainment(float baseFrequency, float entrainmentFrequency)
    {
        frequency1 = baseFrequency;
        frequency2 = baseFrequency + entrainmentFrequency;

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
            // TODO make practical
            if (data[i] == 0 && data[i + 1] == 0 && timeIndex != 0 && timeIndex > 10000000)
            {
                Debug.Log("reset time index");
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
