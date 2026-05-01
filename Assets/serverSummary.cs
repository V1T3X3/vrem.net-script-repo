using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class serverSummary : MonoBehaviour
{
    [SerializeField] TMP_Text ipconfbox;
    [SerializeField] TMP_Text webserverbox;
    [SerializeField] TMP_Text namebox;
    [SerializeField] TMP_Text serverbox;
    [SerializeField] ipaddresser addressing;
    [SerializeField] WebServer webserv;
    [SerializeField] domainName nameserver;
    [SerializeField] DhcpService addserver;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        ipconfbox.text = $"Addressing \n" +
            $"IP Address:\n{addressing.getRawString()} \n \n" +
            $"Mask Length:\n/{addressing.maskLength} \n \n" +
            $"Gateway Address: {addressing.gateway}";
        webserverbox.text = $"Web Server \n" +
            $"Running: {webserv.returnRunning()} \n \n" +
            $"Available Images: {getAllPNGs()}";
        namebox.text = $"Name Server \n" +
            $"Running: {nameserver.getRunning()} \n \n" +
            $"DNS Records: {getNames()}";
        serverbox.text = $"DHCP Server \n" +
            $"Running: {addserver.isRunning} \n \n" +
            $"Pools: {getAllPools()}";
    }

    private string getAllPNGs()
    {
        string temp = string.Empty;
        foreach (string pathText in webserv.getAllPNGPaths())
        {
            temp += Path.GetFileNameWithoutExtension(pathText) + "\n";
        }
        return temp;
    }

    private string getNames()
    {
        string temp = string.Empty;
        foreach (KeyValuePair<string, string> mapping in nameserver.getMaps())
        {
            temp += $"[{mapping.Key}, \n{mapping.Value}]\n";
        }
        return temp;
    }

    private string getAllPools()
    {
        string temp = string.Empty;
        foreach (DhcpPool pool in addserver.pools)
        {
            temp += $"{BinaryToDottedDecimal(pool.networkBin)}/{pool.maskLength.ToString()}\n";
        }
        return temp;
    }

    public string BinaryToDottedDecimal(string binary)
    {
        
        try
        {
            string octet1 = Convert.ToByte(binary.Substring(0, 8), 2).ToString();
            string octet2 = Convert.ToByte(binary.Substring(8, 8), 2).ToString();
            string octet3 = Convert.ToByte(binary.Substring(16, 8), 2).ToString();
            string octet4 = Convert.ToByte(binary.Substring(24, 8), 2).ToString();

            return $"{octet1}.{octet2}.{octet3}.{octet4}";
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to convert binary to decimal.", ex);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
