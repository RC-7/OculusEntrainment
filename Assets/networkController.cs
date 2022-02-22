using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class networkController : MonoBehaviour
{
    private float refreshRate;

    private string serverResponse;
    private AWSResourceValues awsResourceValues;
    private SettingsObject settings;
    private string entrainmentDataType = "entrainmentSettings";

    private void updateFlashing()
    {
        GameObject visualEntrainmentObject = GameObject.Find("visualEntrainment");
        visualEtrainment visualEtrainmentsScript = (visualEtrainment)visualEntrainmentObject.GetComponent(typeof(visualEtrainment));
        visualEtrainmentsScript.setRefresh(refreshRate);
    }

    // Updates settings of current entrainment if they are due to be changed
    private void updateEntrainment()
    {
        // Upodate refreshRate
        // Update audio frequnecy
    }

    public static T ImportJson<T>(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        Debug.Log(textAsset.text);
        return JsonUtility.FromJson<T>(textAsset.text);
    }

    public static T ImportJsonString<T>(string jsonData)
    {
        return JsonUtility.FromJson<T>(jsonData);
    }

    IEnumerator makeRequest()
    {
        // Populate form with fields to make request to API
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
            }
            else
            {
                serverResponse = www.downloadHandler.text;
                Debug.Log(www.downloadHandler.text);
                string settingJSONString = www.downloadHandler.text.Substring(23, www.downloadHandler.text.Length - 25);
                Debug.Log(settingJSONString);
                //SettingsObject settingsRecieved = ImportJson<SettingsObject>(settingJSONString);
                SettingsObject settingsRecieved = ImportJsonString<SettingsObject>(settingJSONString);
                Debug.Log(settingsRecieved.visual.frequency);
            }
        }
    }

    // Querries settings from API on AWS every 5 min
    private void querrySettings()
    {
        StartCoroutine(makeRequest()); // Allows the coroutine to be non blocking
    }
    void Start()
    {
        awsResourceValues = ImportJson<AWSResourceValues>("aws_resources");
        refreshRate = 2.0f; // Pull in from a common file for all settings
        InvokeRepeating("querrySettings", 1.0f, 30.0f);
    }

void Update()
    {
        
    }
}
