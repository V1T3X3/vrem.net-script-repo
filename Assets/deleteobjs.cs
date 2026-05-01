using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deleteobjs : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] DeletePopulate popper;
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
            {
                temp.Add(child.gameObject);
            }
        }
        foreach (var gob in temp)
        {
            GameObject holder = GameObject.Find(gob.GetComponentInChildren<Text>().text);
            Destroy(gob);
            holder.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
