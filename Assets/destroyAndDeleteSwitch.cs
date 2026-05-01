using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class destroyAndDeleteSwitch : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] switchBV switcher;
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
            switcher.destroyVlan(gob.GetComponentInChildren<Text>().text);
            Destroy(gob);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
