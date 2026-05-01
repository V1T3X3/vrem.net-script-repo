using System.Collections.Generic;
using UnityEngine;

public class vlanList : MonoBehaviour
{
    public string listname = string.Empty;
    public List<string> knownActive = new List<string>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void setName(int num)
    {
        this.listname = num.ToString().PadLeft(4, '0');
    }

    public bool searchName(int num)
    {
        string buffer = num.ToString().PadLeft(4, '0');
        if (buffer == listname)
            return true;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
