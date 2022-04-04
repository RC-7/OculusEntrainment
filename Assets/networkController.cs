using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class networkController : MonoBehaviour
{
    private float refreshRateNormal = 60.0f;
    private float shortPoll = 15.0f;
    private bool normalPoll = true;
    private string serverResponse;
    private AWSResourceValues awsResourceValues;
    private SettingsObject settings;
    private string entrainmentDataType = "entrainmentSettings";

    // Add update to color of ball
    private void updateVisualEntrainment()
    {
        GameObject visualEntrainmentObject = GameObject.Find("visualEntrainment");
        visualEtrainment visualEtrainmentsScript = (visualEtrainment)visualEntrainmentObject.GetComponent(typeof(visualEtrainment));
        float refreshRateHz = float.Parse(settings.visual.frequency);
        float refreshRateS = 1 / refreshRateHz;
        visualEtrainmentsScript.setRefresh(refreshRateS);
    }

    private void updateAudioEntrainment()
    {
        GameObject audioEntrainmentObject = GameObject.Find("audioEntrainment");
        audioEntrainment audioEntrainmentScript = (audioEntrainment)audioEntrainmentObject.GetComponent(typeof(audioEntrainment));
        float baseFrequency = float.Parse(settings.audio.baseFrequency);
        float entrainmentFrequency = float.Parse(settings.audio.entrainmentFrequency);
        audioEntrainmentScript.UpdateAudioEntrainment(baseFrequency, entrainmentFrequency);
    }

    private void updateNeurofeedback()
    {
        GameObject neurofeedbackObject = GameObject.Find("neurofeedback");
        neurofeedback neurofeedbackScript = (neurofeedback)neurofeedbackObject.GetComponent(typeof(neurofeedback));
        float redChannel = float.Parse(settings.neurofeedback.redChannel)/255;
        float greenChannel = float.Parse(settings.neurofeedback.greenChannel)/255;
        neurofeedbackScript.UpdateColour(redChannel, greenChannel);
    }

    // Updates settings of current entrainment if they are due to be changed
    private void updateEntrainment()
    {
        updateVisualEntrainment();
        updateAudioEntrainment();
        updateNeurofeedback();
    }

    public static T ImportJson<T>(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        return JsonUtility.FromJson<T>(textAsset.text);
    }

    public static T ImportJsonString<T>(string jsonData)
    {
        return JsonUtility.FromJson<T>(jsonData);
    }

    IEnumerator makeRequest()
    {
        RequestBody body = new RequestBody();

        body.projectionAttributes = "customEntrainment";
        string json = JsonUtility.ToJson(body);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);


        string apiURL = awsResourceValues.get_data_url.value;
        string apiKey = awsResourceValues.get_data_api_key.value;

        using (UnityWebRequest www = UnityWebRequest.Post(apiURL, ""))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("x-api-key", apiKey);
            www.SetRequestHeader("dataType", entrainmentDataType);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                // Refresh sooner
                Debug.Log(www.error);
                CancelInvoke();
                InvokeRepeating("querrySettings", 1.0f, shortPoll);
                normalPoll = false;
            }
            else
            {
                serverResponse = www.downloadHandler.text;
                Debug.Log(www.downloadHandler.text);
                string settingJSONString = www.downloadHandler.text.Substring(23, www.downloadHandler.text.Length - 25);
                Debug.Log(settingJSONString);
                SettingsObject settingsRecieved = ImportJsonString<SettingsObject>(settingJSONString);
                Debug.Log(settingsRecieved.visual.frequency);
                if (JsonUtility.ToJson(settingsRecieved) != JsonUtility.ToJson(settings))
                {
                    settings = settingsRecieved;
                    updateEntrainment();
                    if (!normalPoll)
                    {
                        CancelInvoke();
                        InvokeRepeating("querrySettings", 1.0f, refreshRateNormal);
                    }

                }

            }
        }
    }

    // Querries settings from API on AWS every 5 min in normal operation
    private void querrySettings()
    {
        StartCoroutine(makeRequest()); // Allows the coroutine to be non blocking
    }
    void Start()
    {
        awsResourceValues = ImportJson<AWSResourceValues>("aws_resources");
        InvokeRepeating("querrySettings", 1.0f, refreshRateNormal);
    }

void Update()
    {
        
    }
}
