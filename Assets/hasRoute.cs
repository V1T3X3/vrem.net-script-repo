using System;
using System.Collections.Generic;
using UnityEngine;

public class hasRoute : MonoBehaviour
{
    public List<(string sourceNet, string sourceMask, string hopAdd)> routes = new List<(string, string, string)>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void setRoute(string rawString, int num, string hops)
    {
        routes.Add((convertRawtoBin(rawString), setMaskBin(convertRawtoBin(rawString), num), convertRawtoBin(hops)));
    }

    public string convertRawtoBin(string source)
    {
        string lag = null;
        string[] buff = source.Split('.');
        for (int i = 0; i < buff.Length; i++)
        {
            lag += Convert.ToString(Convert.ToInt32(buff[i].ToString()), 2).PadLeft(8, '0');
        }
        return lag;
    }

    public string setMaskBin(string buff, int len)
    {
        string maskBin = null;
        for (int i = 0; i < len; i++)
        {
            maskBin += buff.ToCharArray()[i].ToString();
        }
        string buffer = maskBin.PadRight(32, '0');
        return buffer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
