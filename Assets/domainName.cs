using System.Collections.Generic;
using UnityEngine;

public class domainName : MonoBehaviour
{
    public Dictionary<string, string> mapping = new Dictionary<string, string>();
    private bool isRunning;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
    }

    public void deleteAllMaps()
    {
        mapping.Clear();
        isRunning = false;
    }

    public bool getRunning()
    {
        return isRunning;
    }

    public void toggleActive()
    {
        if (isRunning)
        {
            isRunning = false;
        }
        else
            isRunning = true;
    }

    public void setMapping(string domainString, string rawIp)
    {
        if (!mapping.ContainsKey(domainString) || !mapping.ContainsValue(rawIp))
            mapping.Add(domainString, rawIp);
    }

    public string getMapping(string domainString)
    {
        if (isRunning)
            return mapping[domainString];
        return null;
    }

    public Dictionary<string, string> getMaps()
    {
        return mapping;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
