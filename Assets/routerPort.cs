using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static UnityEngine.GraphicsBuffer;

public class routerPort : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor sock;
    [SerializeField] ipaddresser addresser;
    [SerializeField] UniqueID portId;
    [SerializeField] private routerBV hostRouter;
    public string helperAddress = null;
    private GameObject connectedObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(updateself), 0f, 1f);
    }

    public string callPortId()
    {
        return portId.GUID;
    }

    public ipaddresser getAddresser()
    {
        return addresser;
    }

    public void createSubInt(string name)
    {
        routerSubInter rb = this.gameObject.AddComponent<routerSubInter>();
        rb.setName(name);
    }
    public void updateself()
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

    public bool networkOut(string targetRawString)
    {
        Debug.Log(targetRawString);
        string binRaw = addresser.convertRawtoBin(targetRawString);

        try
        {
            string binMask = addresser.ConvMaskBin(targetRawString, addresser.maskLength);
            if (addresser.communicable(binRaw, binMask))
                return true;
        }
        catch (Exception)
        {
            Debug.LogWarning("searchingSubs");
            foreach (var subs in GetComponents<routerSubInter>())
            {
                if (subs.subIntOut(targetRawString))
                    return true;
            }

        }
        return false;
    }

    public bool vlanMapped(string gatewat, string vlanName)
    {
        foreach (var subs in GetComponents<routerSubInter>())
        {
            if (subs.encapLan == vlanName && addresser.convertRawtoBin(gatewat) == subs.addressing.binString)
                return true;
        }
        return false;
    }

    public string relaytarget(string stringtart)
    {
        return hostRouter.returnAddress(stringtart);
    }

    public bool VlanMapExists(string stringert, string vlanName)
    {
        return hostRouter.VlanMapExists(stringert, vlanName);
    }

    public bool helperExists(string vlanName = "0001")
    {
        if (helperAddress != null && vlanName == "0001")
        {
            return true;
        }
        else if (vlanName != "0001")
        {
            foreach (var subs in GetComponents<routerSubInter>())
            {
                if (subs.encapLan == vlanName && subs.helperAddre != null)
                    return true;
            }
        }
        return false;
    }

    public string dhcpSearch(string vlanName = "0001")
    {
        ipaddresser tempoaddresser = addresser;
        string tempor = string.Empty;
        Debug.Log(vlanName);
        if ((helperAddress == "" && addresser.getRawString() == "") || vlanName != "0001")
        {
            foreach (var subs in GetComponents<routerSubInter>())
            {
                if (subs.encapLan == vlanName && subs.helperAddre != null)
                {
                    tempor = relaytarget(subs.helperAddre);
                    tempoaddresser = subs.addressing;
                }
            }
        }
        else 
            tempor = relaytarget(helperAddress);
        if (manager.FindByGuid(tempor).TryGetComponent<DhcpService>(out DhcpService dhcp) && dhcp.isRunning)
        {
            return dhcp.requestAddress(tempoaddresser);
        }
        return null;
    }

    public void setHelperAddress(string helperAd)
    {
        helperAddress = helperAd;
    }

    public string getTarget(string target)
    {
        foreach (var subs in GetComponents<routerSubInter>())
        {
            if (connectedObj.TryGetComponent<switchBV>(out switchBV listas) && subs.subIntOut(target))
            {
                HashSet<string> keys = new HashSet<string>();
                string temp = listas.getTargetwithVlan(subs.addressing, target, subs.encapLan, keys);
                Debug.LogWarning($"{temp}/{target}");
                Debug.LogWarning($"{subs.addressing.getRawString()}");
                if (manager.FindByGuid(temp).TryGetComponent<pcBV>(out pcBV host) && 
                     host.isHostTargetable(subs.addressing, target) && 
                     host.compareHostGateway(subs.addressing.getRawString()))
                {
                    return host.machineId.GUID;
                }
                else if (manager.FindByGuid(temp).TryGetComponent<ServerBV>(out ServerBV servicer) &&
                        host.isHostTargetable(subs.addressing, target) &&
                     host.compareHostGateway(subs.addressing.getRawString()))
                {
                    return servicer.serverIdComp().GUID;
                }
            }
            
        }
        if (connectedObj.TryGetComponent<switchBV>(out switchBV lista))
        {
                HashSet<string> keyser = new HashSet<string>();
                string temp = lista.getTarget(addresser, target, portId, keyser);
                if (manager.FindByGuid(temp).TryGetComponent<pcBV>(out pcBV hoster) &&
                hoster.isHostTargetable(addresser, target) && 
                hoster.compareHostGateway(addresser.getRawString()))
                    return temp;
                else if (manager.FindByGuid(temp).TryGetComponent<ServerBV>(out ServerBV servicer) && 
                manager.FindByGuid(temp).TryGetComponent<ipaddresser>(out ipaddresser adressers)
                    && servicer.isHostTargetable(addresser, target) && 
                    servicer.compareHostGateway(addresser.getRawString()))
                {
                    return temp;
                }
        }
        else if (connectedObj.TryGetComponent<pcBV>(out pcBV host) && 
            host.isHostTargetable(addresser, target) && 
            host.compareHostGateway(addresser.getRawString()))
        {
                return host.machineId.GUID;
        }
        else if (connectedObj.TryGetComponent<ServerBV>(out ServerBV servuicaer) &&
            servuicaer.isHostTargetable(addresser, target) && 
            servuicaer.compareHostGateway(addresser.getRawString()))
        {
            Debug.Log("foundserverthrutraverse");
            return servuicaer.serverIdComp().GUID;
        }
        return null;
    }


    // Update is called once per frame
    void Update()
    {
    }
}
