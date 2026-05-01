using System;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
//using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ServerBV : MonoBehaviour, IResettable
{
    [SerializeField] XRSocketInteractor sock;
    [SerializeField] UniqueID serverId;
    [SerializeField] ipaddresser addresser;
    [SerializeField] WebServer webservice;
    [SerializeField] domainName nameservice;
    [SerializeField] DhcpService addressServicer;
    [SerializeField] UnityEngine.UI.Toggle toggleWeb;
    [SerializeField] TMP_InputField addr;
    [SerializeField] TMP_InputField masktext;
    [SerializeField] TMP_InputField gateway;
    [SerializeField] TMP_InputField dns;
    [SerializeField] TMP_InputField domainName;
    [SerializeField] TMP_InputField serverAddress;
    [SerializeField] TMP_InputField dhcpNetAdd;
    [SerializeField] TMP_InputField dhcpNetMask;
    [SerializeField] TMP_InputField dhcpGateAdd;
    [SerializeField] TMP_InputField dhcpDnsAdd;
    [SerializeField] TMP_InputField dhcpLeaseTime;
    [SerializeField] private GameObject UIGameObject;
    private GameObject connectedObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(getCon), 0f, 1f);    
    }

    public void ResetToDefault()
    {
        addresser.clearInfo();
        webservice.deleteDefault();
        nameservice.deleteAllMaps();
        destroyAllPools();
        addr.text = "";
        masktext.text = "";
        gateway.text = "";
        dns.text = "";
        domainName.text = "";
        serverAddress.text = "";
        dhcpNetAdd.text = "";
        dhcpNetMask.text = "";
        dhcpGateAdd.text = "";
        dhcpDnsAdd.text = "";
        dhcpLeaseTime.text = "";
        toggleWeb.enabled = false;
        connectedObj = null;
        resetUI();
    }

    public void resetUI()
    {
        foreach (Transform child in UIGameObject.transform)
        {
            if (child.gameObject.name == "intScreen")
                child.gameObject.SetActive(true);
            else
                child.gameObject.SetActive(false);
        }

    }

    public void destroyAllPools()
    {
        List<DhcpPool> temp = new List<DhcpPool>();
        foreach (DhcpPool tempo in addressServicer.pools)
        {
            temp.Add(tempo);
        }
        foreach (DhcpPool tempter in temp)
        {
                addressServicer.pools.Remove(tempter);
                Destroy(tempter);
        }
        addressServicer.isRunning = false;
    }

    public void getCon()
    {
        if (sock.hasSelection &&
            sock.GetOldestInteractableSelected().transform.gameObject.TryGetComponent<getSib>(out getSib han) &&
            han.getcast() != null)
        {
            connectedObj = manager.FindByGuid(han.getcast());
        }
        else
        {
            connectedObj = null;
        }
    }

    public void destroyNames(string mapps)
    {
        Dictionary<string, string> temp = new Dictionary<string, string>();
        foreach (var objs in nameservice.mapping)
        {
            temp.Add(objs.Key, objs.Value);
        }

            if (temp.ContainsKey(mapps))
            {
                nameservice.mapping.Remove(mapps);
            }        
    }

    public bool isHostTargetable(ipaddresser fold, string targie)
    {
        if (addresser.targetable(fold, targie))
            return true;
        return false;
    }

    public bool compareHostGateway(string targie)
    {
        if (addresser.gateway == targie)
            return true;
        return false;
    }

    public void destroyPools(string binaryAdd)
    {
        List<DhcpPool> temp = new List<DhcpPool>();
        foreach (DhcpPool tempo in addressServicer.pools)
        {
            temp.Add(tempo);
        }
        foreach (DhcpPool tempter in temp)
        {
            if (tempter.networkBin == binaryAdd)
            {
                addressServicer.pools.Remove(tempter);
                Destroy(tempter);
            }
        }
    }

    public void createDhcpPool()
    {
        if (dhcpNetAdd.text != "" && dhcpNetMask.text != "" && dhcpLeaseTime.text != "")
            addressServicer.createNewPoolwithCheck(dhcpNetAdd.text, Convert.ToInt32(dhcpNetMask.text), dhcpDnsAdd.text, dhcpGateAdd.text, Convert.ToInt32(dhcpLeaseTime.text));
    }

    public UniqueID serverIdComp()
    {
        return serverId;
    }
    
    public void setMaps()
    {
        if (domainName.text != "" && serverAddress.text != "")
            nameservice.setMapping(domainName.text, serverAddress.text);
    }
    
    public void toggleWebServ()
    {
        if (toggleWeb.isOn)
            webservice.toggleActive();
        else webservice.toggleActive();
    }

    public void setIp()
    {
        try
        {
            if (addr.text != "" && masktext.text != "")
            {
                addresser.setRawString(addr.text);
                addresser.setMaskLength(Convert.ToInt32(masktext.text));
                addresser.gateway = gateway.text;
            }
        }
        catch
        { 
        }
    }

    

    public string returnImage()
    {
        return webservice.getSite();
    }

    public bool reachable(string targetId, string targetidentity)
    {
        if (connectedObj.TryGetComponent<switchBV>(out switchBV lista))
        {
            HashSet<string> keys = new HashSet<string>();
            string temp = lista.getTarget(addresser, targetId, serverId, keys);
            if (manager.FindByGuid(temp).TryGetComponent<UniqueID>(out UniqueID host) && host.GUID == targetidentity)
                return true;
        }
        else if (connectedObj.TryGetComponent<routerPort>(out routerPort rote) && rote.relaytarget(targetId) != null &&
                manager.FindByGuid(rote.relaytarget(targetId)).TryGetComponent<pcBV>(out pcBV hoster) && hoster.machineId.GUID == targetidentity)
        {
            return true;
        }
        else if (connectedObj.TryGetComponent<pcBV>(out pcBV host) &&
            host.machineId.GUID == targetidentity)
        {
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
