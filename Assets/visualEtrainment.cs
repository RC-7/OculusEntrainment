using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visualEtrainment : MonoBehaviour
{
    private float refreshRate;
    private int timeToStartRefresh;

    void flash()    // Look at syncronising this with the audio, i.e. when the audio is say above 70 % or something of the sign in update then we flash or something similar GG's that would solve the issue completely!! Can do in the controller class
    {
        GameObject visualEntrainmentObject = GameObject.Find("visualEntrainment");

        if (visualEntrainmentObject.transform.localScale.y <= 0)
        {
            visualEntrainmentObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (visualEntrainmentObject.transform.localScale.y > 0)
        {
            visualEntrainmentObject.transform.localScale = new Vector3(0, 0, 0);
        }

    }

    public void setRefresh(float refreshUpdate)
    {
        refreshRate = refreshUpdate;
        CancelInvoke();
        InvokeRepeating("flash", timeToStartRefresh, refreshRate);
    }

    void Start()
    {
        refreshRate = 0.1f;
        timeToStartRefresh = 0;

        InvokeRepeating("flash", timeToStartRefresh, refreshRate);
        Debug.Log("I am alive!");

    }

    void Update()
    {
    }
}
