using System.Collections.Generic;
using UnityEngine;

public class DhcpService : MonoBehaviour
{
    public List<DhcpPool> pools = new List<DhcpPool>();
    System.Random rand = new System.Random();
    public bool isRunning = false;
    [SerializeField] ipaddresser addressing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void createNewPoolwithCheck(string rawAddressString, int Netmask, string DnsAddress, string gatewayAddress, int leaseDurationinSeconds)
    {
        bool isExisting = false;
        foreach (DhcpPool pool in pools)
        {
            if (pool.networkBin == addressing.ConvMaskBin(rawAddressString, Netmask))
            {
                isExisting = true;
            }
        }
        if (isExisting || rawAddressString == "" || Netmask == 0 ||
            leaseDurationinSeconds == 0) {
            Debug.Log("pool exists");
        }
        else
        {
            var temp = this.gameObject.AddComponent<DhcpPool>();
            temp.setup(rawAddressString, Netmask, DnsAddress, gatewayAddress, leaseDurationinSeconds);
            pools.Add(temp);
        }
    }

    public void createNewPool(string rawAddressString, int Netmask, string DnsAddress, string gatewayAddress, int leaseDurationinSeconds)
    {
        var temp = this.gameObject.AddComponent<DhcpPool>();
        temp.setup(rawAddressString, Netmask, DnsAddress, gatewayAddress, leaseDurationinSeconds);
        pools.Add(temp);
    }

    public void toggleRunning()
    {
        if (isRunning)
            isRunning = false;
        else
            isRunning = true;
    }

    public bool acknowledgeRequest(ipaddresser dresser)
    {
        foreach (DhcpPool pool in pools)
        {
            if (dresser.maskBin == pool.networkBin && pool.existsInPool(dresser.binString))
            { 
                       return true;
            }

        }
        return false;
    }

    public void refreshLease(ipaddresser dresser)
    {
        Debug.Log("refresh called");
        foreach (DhcpPool pool in pools)
        {
            if (dresser.maskBin == pool.networkBin && pool.existsInPool(dresser.binString))
            {
                pool.RefreshTimer(dresser.binString);
            }

        }
    }

    public string requestAddress(ipaddresser dresser)
    {
        string temp = null;
        if (isRunning && addressing.getRawString() != "")
        {
            if (dresser.binString == "" && dresser.maskBin == "")
            {
                Debug.LogWarning("Dhcp call with ineligible address");
                foreach (DhcpPool pool in pools)
                {
                    if (addressing.maskBin == pool.networkBin)
                    {
                        temp = pool.getAddress() + " ";
                        temp += pool.maskLength.ToString() + " ";
                        temp += pool.dns + " ";
                        temp += pool.gateway + " ";
                        temp += pool.expirationInMs() + " ";
                        temp += addressing.getRawString();
                    }
                }
                /*
                int tempo = rand.Next(pools.Count);
                temp = pools[tempo].getAddress() + " ";
                temp += pools[tempo].maskLength.ToString() + " ";
                temp += pools[tempo].dns + " ";
                temp += pools[tempo].gateway + " ";
                temp += addressing.getRawString();*/
            }
            else
            {
                Debug.LogWarning("Dhcp call with legible address");
                foreach (DhcpPool pool in pools)
                {
                    if (dresser.maskBin == pool.networkBin)
                    {
                        temp = pool.getAddress() + " ";
                        temp += pool.maskLength.ToString() + " ";
                        temp += pool.dns + " ";
                        temp += pool.gateway + " ";
                        temp += pool.expirationInMs() + " ";
                        temp += addressing.getRawString();
                    }
                }
            }
        }
        
        return temp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
