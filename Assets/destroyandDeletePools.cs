using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class destroyandDeletePools : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] ServerBV server;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public string ConvMaskBin(string rawInputString, int ownMaskLength)
    {
        string lag = null;
        string buffs = null;
        string[] buff = rawInputString.Split('.');
        for (int i = 0; i < buff.Length; i++)
        {
            lag += Convert.ToString(Convert.ToInt32(buff[i].ToString()), 2).PadLeft(8, '0');
        }
        for (int i = 0; i < ownMaskLength; i++)
        {
            buffs += lag.ToCharArray()[i].ToString();
        }
        string buffer = buffs.PadRight(32, '0');
        return buffer;
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
            string[] temper = gob.GetComponentInChildren<Text>().text.Split('/');
            string result = ConvMaskBin(temper[0], Convert.ToInt32(temper[1]));
            server.destroyPools(result.Trim());
            Destroy(gob);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
