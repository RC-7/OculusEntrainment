using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visualEtrainment : MonoBehaviour
{
    private float refresh;

    void flash()    
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

    // Start is called before the first frame update
    void Start()
    {
        refresh = 0.1f;

        InvokeRepeating("flash", refresh, refresh);
        Debug.Log("I am alive!");

    }

    // Update is called once per frame
    void Update()
    {
        /*CancelInvoke();*/
    }
}
