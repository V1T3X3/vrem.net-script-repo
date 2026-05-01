using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DhcpPool : MonoBehaviour
{
    private string networkRaw;
    public string networkBin;
    public int maskLength;
    public string dns;
    public string gateway;
    private List<string> pool = new List<string>();
    private List<string> available = new List<string>();
    private Dictionary<string, Timer> timers = new Dictionary<string, Timer>();
    private int expirationMs;
    System.Random random = new System.Random();
    public void inputExpirationSeconds(int expirationSeconds)
    {
        expirationMs = expirationSeconds * 1000;
    }

    public string expirationInMs()
    {
        return expirationMs.ToString();
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

    public bool existsInPool(string address)
    {
        if (pool.Contains(address))
            return true;
        return false;
    }

    public void setup(string networkRawInput, int maskLengthInput, string dnsAddress, string gatewayAddress, int expirationInSeconds)
    {
        networkRaw = networkRawInput;
        maskLength = maskLengthInput;
        gateway = gatewayAddress;
        dns = dnsAddress;
        networkBin = ConvMaskBin(networkRawInput, maskLengthInput);
        inputExpirationSeconds(expirationInSeconds);
        fillPool();

    }

    public string getAddress()
    {
        string temp = available[random.Next(available.Count)];
        Add(temp);
        return temp;
    }

    public void fillPool()
    {
        string temp = null;
        while (true)
        {
            temp = generateAddress();
            if (!available.Contains(temp) && temp != null)
            {
                available.Add(temp);
            }
            else if (available.Count >= (Math.Pow(2f, Convert.ToDouble(32 - maskLength)) - 2))
                break;
        }
    }

    private string generateAddress()
    {
        string temp = null;
        string checkBroads = null;
        string checkNets = null;
        for (int i = 0; i < (32 - maskLength); i++)
        {
            temp += random.Next(2).ToString();
            checkBroads += "1";
            checkNets += "0";
        }
        if (temp == checkBroads || temp == checkNets)
            return null;
        char[] networkChars = networkBin.ToCharArray();
        string binaryAdd = null;
        for (int i = 0; i < maskLength; i++)
        {
            binaryAdd += networkChars[i].ToString();
        }
        return binaryAdd + temp;
    }

    public void Add(string item)
    {
        lock (pool)
        {
            pool.Add(item);
            available.Remove(item);
            Timer timer = new Timer(state =>
            {
                Remove(item);
                available.Add(item);
            }, null, expirationMs, Timeout.Infinite);
            timers[item] = timer;
            Debug.Log($"Added {item}, will expire in {expirationMs} ms");
        }
    }

    public bool Remove(string item)
    {
        lock (pool)
        {
            bool removed = pool.Remove(item);
            if (removed)
            {
                timers[item]?.Dispose();
                timers.Remove(item);
                Debug.Log($"Removed {item}");
            }
            return removed;
        }
    }

    public void RefreshTimer(string item)
    {
        lock (pool)
        {
            if (timers.TryGetValue(item, out Timer existingTimer))
            {
                existingTimer.Dispose();

                Timer newTimer = new Timer(state =>
                {
                    Remove(item);
                    available.Add(item);
                }, null, expirationMs, Timeout.Infinite);

                timers[item] = newTimer;

                Debug.Log($"Timer refreshed for {item}, will expire in {expirationMs} ms");
            }
            else
            {
                Debug.LogWarning($"No timer found for {item}. Cannot refresh.");
            }
        }
    }

    public List<string> GetItems()
    {
        lock (pool)
        {
            return new List<string>(pool);
        }
    }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }


        // Update is called once per frame
        void Update()
        {

        }
    public void Dispose()
    {
        lock (pool)
        {
            foreach (var timer in timers.Values)
                timer?.Dispose();
            timers.Clear();
        }
        
    }
}
