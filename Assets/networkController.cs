using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class networkController : MonoBehaviour
{
    private float refreshRate;

    private string serverResponse;

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

    IEnumerator makeRequest()
    {
        // Populate form with fields to make request to API
        WWWForm form = new WWWForm();
        form.AddField("myField", "myData");

        using (UnityWebRequest www = UnityWebRequest.Post("https://www.google.com/index.html", form)) // Pull from a common file
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                serverResponse = www.downloadHandler.text;
                Debug.Log(www.downloadHandler.text);
                Debug.Log("Form upload complete!");
            }
        }
    }

    // Querries settings from API on AWS every 30 s
    private async void querrySettings()
    {
        StartCoroutine(makeRequest()); // Allows the coroutine to be non blocking
    }
    void Start()
    {
        refreshRate = 2.0f; // Pull in from a common file for all settings
        InvokeRepeating("querrySettings", 10.0f, 30.0f);
}

void Update()
    {
        
    }
}
