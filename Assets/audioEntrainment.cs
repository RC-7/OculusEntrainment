using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;


public class audioEntrainment : MonoBehaviour
{
    public float frequency1;
    public float frequency2;
    public float sampleRate = 48000;
    System.Random rng = new System.Random();
    AudioSource audioSource;
    int timeIndex = 0;
    public AudioClip pinkNoiseWav;
    List<float> pinkNoise = new List<float>();

    void Start()
    {
        // Defaults to Alpha waves
        frequency1 = 450.0f;
        frequency2 = 460.0f;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        pinkNoiseWav = Resources.Load<AudioClip>("pinkNoise");
        audioSource.clip = pinkNoiseWav;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;
        audioSource.spatialBlend = 0; //force 2D sound
        audioSource.Stop(); //avoids audiosource from starting to play automatically
        //ReadPinkNoise();
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

    void ReadPinkNoise() 
    {

        using (var rd = new StreamReader("pinkNoise.csv"))
        {
            while (!rd.EndOfStream)
            {
                var value = rd.ReadLine();
                float floatValue = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
                pinkNoise.Add(floatValue);
            }
        }
    }

    void Update()
    {

        if (!audioSource.isPlaying)
        {
            //Debug.Log("Resetting");
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

    void generateBinauralBeat(ref float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            data[i] = CreateSine(timeIndex, frequency1, sampleRate);

            if (channels == 2)
                data[i + 1] = CreateSine(timeIndex, frequency2, sampleRate);

            timeIndex++;
            if ((float)timeIndex == sampleRate * 2.0f)
            {
                //Debug.Log("reset time index");
                timeIndex -= (int)(sampleRate);
            }

        }
    }

    void generatePinkNoise(ref float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            data[i] = pinkNoise[timeIndex];

            if (channels == 2)
                data[i + 1] = pinkNoise[timeIndex];

            timeIndex++;
            if ((float)timeIndex == pinkNoise.Count)
            {
                timeIndex = 0;
            }

        }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (frequency1 != - 1)
        {
            generateBinauralBeat(ref data, channels);
        }
        else 
        {
            //generatePinkNoise(ref data, channels);
        }
    }

    
    public float CreatePinkNoise(int timeIndex, float sampleRate)
    {
        
        float noise_value = 0.0f;
        for (int i = 0; i <= 100; i++)
        {
            float randomPhase = (float)rng.Next(180);
            float amplitudeScaling = (float)System.Math.Pow(i, -0.4);
            noise_value += amplitudeScaling*CreateSine(timeIndex, (float)i, sampleRate, randomPhase);
        }
        return noise_value;
    }
    

    public float CreateSine(int timeIndex, float frequency, float sampleRate, float phase = 0)
    {
        return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate + 2 * Mathf.PI * phase);
    }

}
