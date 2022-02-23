using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class neurofeedback : MonoBehaviour
{ 
    public void UpdateColour(float redChannel, float greenChannel)
    {
        GameObject neurofeedbackObject = GameObject.Find("neurofeedback");
        var neurofeedbackRenderer = neurofeedbackObject.GetComponent<Renderer>();
        Color c = new Color(redChannel, greenChannel, 0f);
        neurofeedbackRenderer.material.SetColor("_Color", c);

    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
