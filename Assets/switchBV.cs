using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using TMPro;
//using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static UnityEngine.GraphicsBuffer;

public class switchBV : MonoBehaviour, IResettable
{
    [SerializeField] lister lister;
    [SerializeField] UniqueID switchId;
    [SerializeField] private GameObject vlanconf;
    [SerializeField] private GameObject portselc;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TMP_Dropdown portType;
    [SerializeField] private TMP_InputField vlancreatenum;
    [SerializeField] private GameObject UIGameObject;
    public string objName;
    private GameObject referen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        createVlan(1);
        InvokeRepeating(nameof(updateTable), 0f, 1f);
        
    }

    public void resetUI()
    {
        foreach (Transform child in UIGameObject.transform)
        {
            if (child.gameObject.name == "portsconf")
                child.gameObject.SetActive(true);
            else
                child.gameObject.SetActive(false);
        }

    }

    public void ResetToDefault()
    {
        destroyAllVlans();
        resetPorts();
        vlancreatenum.text = "";
        referen = null;
        resetUI();
    }

    private void resetPorts()
    {
        foreach (GameObject objs in getAllPorts())
        {
            objs.GetComponent<vlan>().vlanId = 1;
            objs.GetComponent<vlan>().trunking = false;
        }
    }

    private void destroyAllVlans()
    {
        List<vlanList> temp = new List<vlanList>();
        foreach (vlanList objs in lister.knownActive)
        {
            temp.Add(objs);
        }
        foreach (vlanList tempo in temp)
        {

                lister.knownActive.Remove(tempo);
                Destroy(tempo);

        }
    }

    public void destroyVlan(string vlanName)
    {
        List<vlanList> temp = new List<vlanList>();
        foreach (vlanList objs in lister.knownActive)
        {
            temp.Add(objs);
        }
        foreach (vlanList tempo in temp)
        {
            if (tempo.listname == vlanName)
            {
                lister.knownActive.Remove(tempo);
                Destroy(tempo);
            }
        }
    }

    public void updateVlanSelection()
    {
        dropdown.ClearOptions();
        portType.ClearOptions();
        List<string> ops = new List<string>();
        foreach (var vlaner in lister.knownActive)
        {
            ops.Add(vlaner.listname.TrimStart('0'));
        }
        List<string> typs = new List<string>()
        {
            "Access",
            "Trunk"
        };
        portType.AddOptions(typs);
        dropdown.AddOptions(ops);
        if (referen.TryGetComponent<vlan>(out vlan portDet))
        {
            if (portDet.trunking)
            {
                portType.value = 1;
                dropdown.interactable = false;
            }
            else 
            { 
                portType.value = 0;
                dropdown.interactable = true;
            }
                
            for (int i = 0; i < dropdown.options.Count; i++) { 
                if (Convert.ToInt32(dropdown.options[i].text) == portDet.vlanId)
                    dropdown.value = i;
            }
        }
    }

    public void updateTable()
    {
        foreach (var vlaner in lister.knownActive)
        {
            vlaner.knownActive.Clear();
        }
        foreach (var child in getAllPorts())
        {
            if (child.TryGetComponent<XRSocketInteractor>(out XRSocketInteractor sock) &&
                sock.hasSelection && 
                sock.GetOldestInteractableSelected().transform.gameObject.TryGetComponent<getSib>(out getSib sibb) &&
                child.TryGetComponent<vlan>(out vlan lanId))
            {
                foreach (var vlaner in lister.knownActive)
                {
                    if (vlaner.searchName(lanId.vlanId) && !lanId.trunking)
                    {
                        vlaner.knownActive.Add(sibb.getcast());
                    }
                }
            }
        }
    }

    public List<GameObject> getAllPorts()
    {
        List<GameObject> ports = new List<GameObject>();
        foreach (vlan porting in GetComponentsInChildren<vlan>())
            ports.Add(porting.gameObject);
        return ports;
    }

    public void setVlanNum()
    {
        Debug.LogWarning($"{dropdown.options[dropdown.value].text}");
        if (referen != null && Regex.IsMatch(dropdown.options[dropdown.value].text, @"^\d+$")  && referen.TryGetComponent<vlan>(out vlan vlannumber))
            vlannumber.vlanId = Convert.ToInt32(dropdown.options[dropdown.value].text);
    }

    public void setPortType()
    {
        Debug.LogWarning($"{dropdown.options[dropdown.value].text}");
        if (referen != null && portType.options[portType.value].text == "Trunk" && referen.TryGetComponent<vlan>(out vlan vlannumber))
        {
            vlannumber.trunking = true;
            dropdown.interactable = false;
        }
        else
        {
            referen.TryGetComponent<vlan>(out vlan clan);
            clan.trunking = false;
            dropdown.interactable = true;
        }
    }  

    //Deprecated by CreateVlanWithCheck(?) \/
    public List<string> getList(string connObj)
    {
        foreach (var vlaner in lister.knownActive)
        {
            if (vlaner.knownActive.Contains(connObj))
                return vlaner.knownActive;
        }
        return null;
    }


    public void selcVis()
    {
        if (portselc != null && portselc.activeSelf == false)
            portselc.SetActive(true);
        else
            portselc.SetActive(false);

    }

    public void confVisback()
    {
        if (vlanconf.activeSelf == false)
        {
            vlanconf.SetActive(true);
            Debug.LogWarning("fire");
        }
        else
        {
            vlanconf.SetActive(false);
            referen = null;
        }
    }

    public void confVis(GameObject obj)
    {
        if (obj != null && vlanconf.activeSelf == false)
        {
            vlanconf.SetActive(true);
            Debug.LogWarning("fire");
            referen = obj;
        }
        else
        {
            vlanconf.SetActive(false);
            referen = null;
        }
    }

    public string getVlanName(string connObj)
    {
        foreach (var vlaner in lister.knownActive)
        {
            if (vlaner.knownActive.Contains(connObj))
                return vlaner.listname;
        }
        return null;
    }
    public void createVlanwithCheck()
    {
        bool found = false;
        foreach (var vlaner in lister.knownActive)
        {
            if (vlaner.listname == formName( Convert.ToInt32(vlancreatenum.text)))
                found = true;
        }
        if (found)
        { }
        else
        {
            createVlan(Convert.ToInt32(vlancreatenum.text));
        }

    }

    public string formName(int num)
    {
        return num.ToString().PadLeft(4, '0');
    }

    public void createVlan(int num)
    { 
        vlanList rb = this.gameObject.AddComponent<vlanList>();
        rb.setName(num);
        lister.knownActive.Add(rb);
    }

    public vlanList getWithVlanName(string vlanName)
    {
        foreach (var vlaner in lister.knownActive)
        {
            if (vlaner.listname == vlanName)
                return vlaner;
        }
        return null;
    }

    public vlanList vlanDBCheck(string machineId)
    {
        foreach (var vlaner in lister.knownActive)
        {
           if (vlaner.knownActive.Contains(machineId))
                return vlaner;
        }
        return null;
    }

    public string getTargetwithVlan(ipaddresser addresser, string target, string vlanName, HashSet<string> visited)
    {
        visited.Add(switchId.GUID);
        Debug.Log($"{vlanName}");
        foreach (var obj in getWithVlanName(vlanName).knownActive)
        {
            if (visited.Contains(obj))
            {
                Debug.LogWarning($"Circular reference detected! Skipping GUID: {obj}");
                continue;
            }
            visited.Add(obj);
            if (manager.FindByGuid(obj).TryGetComponent<switchBV>(out switchBV lista))
            {
                Debug.LogWarning("InSwitchRange");
                return lista.getTarget(addresser, target, switchId, visited);
            }
            else if (manager.FindByGuid(obj).TryGetComponent<ipaddresser>(out ipaddresser adress) &&
                adress.targetable(addresser, target))
            {
                Debug.LogWarning("FoundTarg");
                return obj;
            }
        }
        if (getallTrunking().Count != 0)
            return getFromTrunks(addresser, target, vlanName, visited);
        return null;
    }

    public string findDhcp( ipaddresser addresser ,UniqueID iterator, HashSet<string> visited)
    {
        visited.Add(switchId.GUID);
        foreach (var obj in vlanDBCheck(iterator.GUID).knownActive)
        {
            if (visited.Contains(obj))
            {
                Debug.LogWarning($"Circular reference detected! Skipping GUID: {obj}");
                continue;
            }
            visited.Add(obj);
            if (manager.FindByGuid(obj).TryGetComponent<switchBV>(out switchBV lista))
            {
                Debug.LogWarning("InSwitchRangeForDhcp");
                return lista.findDhcp(addresser, switchId, visited);
            }
            else if (manager.FindByGuid(obj).TryGetComponent<routerPort>(out routerPort parter) && parter.helperExists())
            {
                Debug.LogWarning("SwitchFoundPortForDhcp");
                return parter.dhcpSearch();
            }
            else if (manager.FindByGuid(obj).TryGetComponent<DhcpService>(out DhcpService servicer) && servicer.isRunning)
            {
                Debug.LogWarning("FoundServer");
                return servicer.requestAddress(addresser);
            }
        }
        if (getallTrunking().Count != 0)
            return getDhcpFromTrunks(addresser, vlanDBCheck(iterator.GUID).listname, visited);
        return null;
    }

    public string getDhcpwithVlan(ipaddresser addresser, string vlanName, HashSet<string> visited)
    {
        visited.Add(switchId.GUID);
        Debug.Log($"{vlanName}");
        foreach (var obj in getWithVlanName(vlanName).knownActive)
        {
            if (visited.Contains(obj))
            {
                Debug.LogWarning($"Circular reference detected! Skipping GUID: {obj}");
                continue;
            }
            visited.Add(obj);
            if (manager.FindByGuid(obj).TryGetComponent<switchBV>(out switchBV lista))
            {
                Debug.LogWarning("InSwitchRange");
                return lista.findDhcp(addresser, switchId, visited);
            }
            else if (manager.FindByGuid(obj).TryGetComponent<routerPort>(out routerPort parter) && parter.helperExists())
            {
                Debug.LogWarning("SwitchFoundPortForDhcp");
                return parter.dhcpSearch();
            }
            else if (manager.FindByGuid(obj).TryGetComponent<DhcpService>(out DhcpService servicer) && servicer.isRunning)
            {
                Debug.LogWarning("FoundServer");
                return servicer.requestAddress(addresser);
            }
        }
        if (getallTrunking().Count != 0)
            return getDhcpFromTrunks(addresser, vlanName, visited);
        return null;
    }

    public string getDhcpFromTrunks(ipaddresser addresser, string vlanName, HashSet<string> visited)
    {
        Debug.LogWarning("DHCP trunking Fired");
        foreach (var temp in getallTrunking())
        {
            temp.TryGetComponent<UniqueID>(out UniqueID objId);
            if (visited.Contains(objId.GUID))
            {
                Debug.LogWarning($"Circular reference detected! Skipping GUID: {objId.GUID}");
                continue;
            }
            if (temp.TryGetComponent<switchBV>(out switchBV switcher) &&
                    switcher.checkIfFromTrunking(switchId.GUID) &&
                    switcher.getWithVlanName(vlanName) != null)
            {
                Debug.LogWarning("Search Now in Trunk Space");
                foreach (var candids in switcher.getWithVlanName(vlanName).knownActive)
                {
                    Debug.Log($"{switcher.switchId}: {vlanName}");
                    if (manager.FindByGuid(candids).TryGetComponent<routerPort>(out routerPort parter) && 
                        parter.helperExists(vlanName))
                    {
                        return parter.dhcpSearch(vlanName);
                    }
                    else if (manager.FindByGuid(candids).TryGetComponent<DhcpService>(out DhcpService servicer) && servicer.isRunning)
                    {
                        Debug.LogWarning("FoundTargUsingTrunks");
                        return servicer.requestAddress(addresser);
                    }
                }
                foreach (var energizer in switcher.getallTrunking())
                {
                    if (energizer.TryGetComponent<routerPort>(out routerPort parter) && parter.helperExists(vlanName))
                    {
                        return parter.dhcpSearch(vlanName);
                    }
                }
                string tempor = switcher.getDhcpwithVlan(addresser, vlanName, visited);
                if (tempor != null)
                {
                    return tempor;
                }
                else
                {
                }
            }
            else if (temp.TryGetComponent<routerPort>(out routerPort prter) && prter.helperExists(vlanName))
            {
                    Debug.LogWarning("HelperAddressFire");
                    return prter.dhcpSearch(vlanName);
            }
            Debug.LogWarning("possible miss on port call");
        }
        return null;
    }

    public string getTarget(ipaddresser addresser, string target, UniqueID iterator, HashSet<string> visited)
    {
        visited.Add(switchId.GUID);
            foreach (var obj in vlanDBCheck(iterator.GUID).knownActive)
            {
                if (visited.Contains(obj))
                {
                    Debug.LogWarning($"Circular reference detected! Skipping GUID: {obj}");
                    continue;
                }
                visited.Add(obj);
                if (manager.FindByGuid(obj).TryGetComponent<switchBV>(out switchBV lista))
                {
                    Debug.LogWarning("InSwitchRange");
                    return lista.getTarget(addresser, target, switchId, visited);
                }
                else if (manager.FindByGuid(obj).TryGetComponent<ipaddresser>(out ipaddresser adress) &&
                    adress.targetable(addresser, target))
                {
                    Debug.LogWarning("FoundTarg");
                    return obj;
                }
            }
        if (getallTrunking().Count != 0)
            return getFromTrunks(addresser, target, vlanDBCheck(iterator.GUID).listname, visited);
        return null;
    }

    public bool checkIfFromTrunking(string machineId)
    {
        foreach (var port in getallTrunking())
        {
            if (port.TryGetComponent<UniqueID>(out UniqueID Id) &&
                Id.GUID == machineId)
                    return true;   
        }
        return false;
    }

    public List<GameObject> getallTrunking()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (var port in GetComponentsInChildren<vlan>())
        {
            if (port.trunking && port.gameObject.TryGetComponent<XRSocketInteractor>(out XRSocketInteractor sock) && sock.hasSelection &&
                sock.GetOldestInteractableSelected().transform.gameObject.TryGetComponent<getSib>(out getSib sibb))
            {
                list.Add(manager.FindByGuid(sibb.getcast()));
            }

        }
        return list;
    }


    public string getFromTrunks(ipaddresser addresser, string target, string vlanName, HashSet<string> visited)
    {
        Debug.LogWarning("trunking Fired");
        foreach (var temp in getallTrunking())
        {
            temp.TryGetComponent<UniqueID>(out UniqueID objId);
            if (visited.Contains(objId.GUID))
            {
                Debug.LogWarning($"Circular reference detected! Skipping GUID: {objId.GUID}");
                continue;
            }
            if (temp.TryGetComponent<switchBV>(out switchBV switcher) && 
                    switcher.checkIfFromTrunking(switchId.GUID) && 
                    switcher.getWithVlanName(vlanName) != null)
                {
                    Debug.LogWarning("Search Now in Trunk Space");
                    foreach (var candids in switcher.getWithVlanName(vlanName).knownActive)
                    {
                        Debug.Log($"{switcher.switchId}: {vlanName}");
                        if (manager.FindByGuid(candids).TryGetComponent<ipaddresser>(out ipaddresser adress) &&
                            adress.targetable(addresser, target))
                        {
                            Debug.LogWarning("FoundTargUsingTrunks");
                            return candids;
                        }
                    }
                    foreach (var energizer in switcher.getallTrunking())
                    {
                        if(energizer.TryGetComponent<routerPort>(out routerPort parter) && parter.VlanMapExists(target, vlanName))
                        {
                            return parter.callPortId();
                        }
                    }
                    string tempor = switcher.getTargetwithVlan(addresser, target, vlanName, visited);
                    if (tempor != null) { 
                        return tempor;
                    }
                    else
                    { 
                    }
                }
            else if (temp.TryGetComponent<routerPort>(out routerPort prter))
            {
                if (prter.VlanMapExists(target, vlanName))
                {
                    Debug.LogWarning("triedPortCalling");
                    return prter.callPortId();
                }
                else if (prter.TryGetComponent<ipaddresser>(out ipaddresser dresser) && 
                    dresser.targetable(addresser, target))
                {
                    return prter.callPortId();
                }
                
            }
            Debug.LogWarning("possible miss on port call");
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
