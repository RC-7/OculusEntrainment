using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class networkController : MonoBehaviour
{
    private float refresh;
    // Start is called before the first frame update

    private void updateFlashing()
    {
        GameObject visualEntrainmentObject = GameObject.Find("visualEntrainment");
        visualEtrainment visualEtrainmentsScript = (visualEtrainment)visualEntrainmentObject.GetComponent(typeof(visualEtrainment));
        visualEtrainmentsScript.setRefresh(refresh);
        refresh+=1.0f;
    }
    void Start()
    {
        refresh = 2.0f;
        InvokeRepeating("updateFlashing", 10.0f, 10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
