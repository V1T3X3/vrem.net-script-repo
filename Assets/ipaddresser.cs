using System;
using TMPro;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class ipaddresser : MonoBehaviour
{
    private string rawString;
    public string binString;
    public int maskLength;
    public string maskBin;
    public string gateway;
    public void rawStringToBinAddress()
    {
        binString = null;
        string[] buff = rawString.Split('.');
        for (int i = 0; i < buff.Length; i++)
        {
            binString += Convert.ToString(Convert.ToInt32(buff[i].ToString()), 2).PadLeft(8, '0');
        }
    }

    public void clearInfo()
    {
        rawString = "";
        binString = "";
        maskLength = 0;
        maskBin = "";
        gateway = "";
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

    public void setRawString(string gens)
    {
        rawString = gens;
        rawStringToBinAddress();
    }

    public string getRawString()
    {
        return rawString;
    }

    public void setMaskBin()
    {
        maskBin = null;
        for (int i = 0; i < maskLength; i++) { 
            maskBin += binString.ToCharArray()[i].ToString();
        }
        string buff = maskBin.PadRight(32, '0');
        maskBin = buff;
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

    public void setMaskLength(int num)
    {
        maskLength = num;
        setMaskBin();
    }

    public string getBinString()
    {
        return binString;
    }

    public string getMaskBin()
    {
        return maskBin;
    }

    public bool communicable(string sourcestring, string sourcemask)
    {
        if (sourcestring != binString && sourcemask == maskBin)
            return true;
        return false;
    }

    public bool targetable(ipaddresser source, string target)
    {
        if (communicable(source.getBinString(), source.getMaskBin()) && convertRawtoBin(target) == binString)
            return true;
        return false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
   
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
