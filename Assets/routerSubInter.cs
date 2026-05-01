using System;
using UnityEngine;

public class routerSubInter : MonoBehaviour
{
    string subIntName;
    public string binIpAddress;
    public string binMask;
    public int maskLengthInt;
    public string encapLan;
    public string helperAddre;
    public ipaddresser addressing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void setHelperAddress(string helperAddr)
    {
        helperAddre = helperAddr.Trim();
    }

    public void setVlanEncap(int num)
    {
        encapLan = num.ToString().PadLeft(4, '0');
    }

    public bool isVlanEncapExist(int num)
    {
        string temp = num.ToString().PadLeft(4, '0');
        if (temp == encapLan)
            return true;
        else 
            return false;
    }

    public void setName(string appending)
    {
        subIntName = this.gameObject.name + $".{appending}";
    }

    public string callName()
    {
        return subIntName;
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

    public void setAddress(string rawAddress, string intMask)
    {
        binIpAddress = convertRawtoBin(rawAddress);
        maskLengthInt = Convert.ToInt32(intMask);
        binMask = ConvMaskBin(rawAddress, Convert.ToInt32(intMask));
        if (addressing == null)
        {
            ipaddresser rb = this.gameObject.AddComponent<ipaddresser>();
            rb.setRawString(rawAddress);
            rb.maskLength = Convert.ToInt32(intMask);
            rb.setMaskBin();
            addressing = rb;
        }
        else
        {
            addressing.setRawString(rawAddress);
            addressing.maskLength = Convert.ToInt32(intMask);
            addressing.setMaskBin();
        }
    }

    public bool subIntOut(string sourcestring)
    {
        
        if (convertRawtoBin(sourcestring) != binIpAddress && ConvMaskBin(sourcestring, maskLengthInt) == binMask)
            return true;
        return false;
    }

    public bool communicable(string sourcestring, string sourcemask)
    {
        if (sourcestring != binIpAddress && sourcemask == binMask)
            return true;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
