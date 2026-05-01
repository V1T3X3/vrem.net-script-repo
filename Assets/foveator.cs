using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class foveator : MonoBehaviour
{
    List<XRDisplaySubsystem> xrDisplays = new List<XRDisplaySubsystem>();



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SubsystemManager.GetSubsystems(xrDisplays);
        if (xrDisplays.Count == 1)
        {
            xrDisplays[0].foveatedRenderingLevel = 1.0f; // Full strength
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
