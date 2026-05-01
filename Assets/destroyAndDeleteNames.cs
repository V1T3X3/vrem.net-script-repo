using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class destroyAndDeleteNames : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] ServerBV server;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void destroyEnabled()
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (UnityEngine.UI.Toggle child in obj.GetComponentsInChildren<UnityEngine.UI.Toggle>())
        {
            if (child.isOn)
                temp.Add(child.gameObject);
        }
        foreach (var gob in temp)
        {
            string temper = gob.GetComponentInChildren<Text>().text;
            int index = temper.IndexOf("->");
            string result = index >= 0 ? temper.Substring(0, index) : temper;
            server.destroyNames(result.Trim());
            Destroy(gob);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
