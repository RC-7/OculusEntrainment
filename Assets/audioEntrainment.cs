using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;


public class audioEntrainment : MonoBehaviour
{
    public float baseFrequency;
    public float entrainedFrequency;
    public float sampleRate = 48000;
    System.Random rng = new System.Random();
    AudioSource audioSource;
    int timeIndex = 0;
    public AudioClip pinkNoiseWav;
    List<float> pinkNoise = new List<float>();

    void Start()
    {
        // Defaults to Alpha waves
        baseFrequency = 450.0f;
        entrainedFrequency = 460.0f;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        pinkNoiseWav = Resources.Load<AudioClip>("pinkNoise");
        audioSource.clip = pinkNoiseWav;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;
        audioSource.spatialBlend = 0;
        audioSource.Stop();
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
            audioSource.Play();
        }
    }

    public void UpdateAudioEntrainment(float baseFrequency, float entrainmentFrequency)
    {
        this.baseFrequency = baseFrequency;
        entrainedFrequency = baseFrequency + entrainmentFrequency;
        Debug.Log(entrainedFrequency);

        if (!audioSource.isPlaying)
        {
            timeIndex = 0;  //resets timer before playing sound
            audioSource.Play();
        }
    }

    void generateBinauralBeat(ref float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            data[i] = CreateSine(timeIndex, baseFrequency, sampleRate);

            if (channels == 2)
                data[i + 1] = CreateSine(timeIndex, entrainedFrequency, sampleRate);

            timeIndex++;
            if ((float)timeIndex == sampleRate * 2.0f)
            {
                timeIndex -= (int)(sampleRate);
            }

        }
    }


    void OnAudioFilterRead(float[] data, int channels)
    {
        if (baseFrequency != - 1)
        {
            generateBinauralBeat(ref data, channels);
        }
        else 
        {
            // Do nothing as the Audio clip has pink noise
        }
    }

    

    public float CreateSine(int timeIndex, float frequency, float sampleRate, float phase = 0)
    {
        return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate + 2 * Mathf.PI * phase);
    }

}
