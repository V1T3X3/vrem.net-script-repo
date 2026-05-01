using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class routerBV : MonoBehaviour, IResettable
{
    [SerializeField] lister lister;
    [SerializeField] private TMP_InputField vlancreatenum;
    [SerializeField] private TMP_InputField subintcreatetext;
    [SerializeField] private hasRoute routeTable;
    [SerializeField] private TMP_InputField ipaddress;
    [SerializeField] private TMP_InputField mask;
    [SerializeField] private TMP_InputField IpAddressSubInt;
    [SerializeField] private TMP_InputField maskSubInt;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TMP_InputField haddr;
    [SerializeField] private TMP_InputField subInthaddr;
    [SerializeField] private GameObject ipconf;
    [SerializeField] private GameObject portselc;
    [SerializeField] private GameObject optMenu;
    [SerializeField] private GameObject Subints;
    [SerializeField] private GameObject subIntConf;
    [SerializeField] private GameObject UIgameobject;
    public GameObject referen;
    private routerSubInter subinttemp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void ResetToDefault()
    {
        deleteAllVlans();
        deleteAllSubints();
        deleteAllPortInfo();
        vlancreatenum.text = "";
        subintcreatetext.text = "";
        ipaddress.text = "";
        mask.text = "";
        IpAddressSubInt.text = "";
        maskSubInt.text = "";
        haddr.text = "";
        subInthaddr.text = "";
        referen = null;
        subinttemp = null;
        resetUI();
    }

    public void resetUI()
    {
        foreach (Transform child in UIgameobject.transform)
        {
            if (child.gameObject.name == "portSelc")
                child.gameObject.SetActive(true);
            else
                child.gameObject.SetActive(false);
        }

    }

    private void deleteAllPortInfo()
    {
        foreach (GameObject ober in returnAllPorts())
        {
            ober.GetComponent<ipaddresser>().clearInfo();
            ober.GetComponent<routerPort>().helperAddress = "";
        }
    }

    private List<GameObject> returnAllPorts()
    {
        List<GameObject> onjec = new List<GameObject>();
        foreach (XRSocketInteractor socks in GetComponentsInChildren<XRSocketInteractor>())
        {
            onjec.Add(socks.gameObject);
        }
        return onjec;
    }

    private void deleteAllVlans()
    {
        List<vlanList> temp = new List<vlanList>();
        foreach (vlanList tempo in lister.knownActive)
        {
            temp.Add(tempo);
        }
        foreach (vlanList tempo in temp)
        {

                lister.knownActive.Remove(tempo);
                Destroy(tempo);

        }
    }

    private void deleteAllSubints()
    {
        routerSubInter[] subber = GetComponentsInChildren<routerSubInter>();
        foreach (routerSubInter subs in subber)
        {
            Destroy(subs.addressing);
            Destroy(subs);
        }
    }

    public void setHelperAddressSubInt()
    {
        if (subinttemp != null)
            subinttemp.helperAddre = subInthaddr.text;
    }

    public void setHelperAddress()
    {
        if (referen.TryGetComponent<routerPort>(out routerPort porter))
            porter.helperAddress = haddr.text;
    }

    public void destroySubInt()
    {
        Destroy(subinttemp.addressing);
        Destroy(subinttemp);
        subinttemp = null;
    }

    public void updateVlanSelection()
    {
        dropdown.ClearOptions();
        List<string> ops = new List<string>();
        List<string> newops = new List<string>();
        List<string> drops = new List<string>();
        foreach (var vlaner in lister.knownActive)
        {
            ops.Add(vlaner.listname.TrimStart('0'));
        }
        foreach (routerSubInter portss in referen.GetComponents<routerSubInter>())
        {
            if (portss.encapLan != null)
                newops.Add(portss.encapLan.TrimStart('0'));
        }
        foreach (string bff in ops)
        {
            if (!newops.Contains(bff))
                drops.Add(bff);
        }
        dropdown.AddOptions(drops);
    }

    public void setSubIntConf()
    {
        if (IpAddressSubInt.text != "" && maskSubInt.text != "")
        {
            subinttemp.setAddress(IpAddressSubInt.text, maskSubInt.text);
            setVlanNum();
        }
    }

    public void setVlanNum()
    {
        Debug.LogWarning($"{dropdown.options[dropdown.value].text}");
        if (Regex.IsMatch(dropdown.options[dropdown.value].text, @"^\d+$") && subinttemp != null)
            subinttemp.setVlanEncap(Convert.ToInt32(dropdown.options[dropdown.value].text));
    }

    public void subConfVisback()
    {
        if (subIntConf.activeSelf == false)
        {
            subIntConf.SetActive(true);
        }
        else
        {
            subIntConf.SetActive(false);
            subinttemp = null;
        }
    }

    public void subConfSelc(routerSubInter script)
    {
        if (subIntConf.activeSelf == false)
        {
            subinttemp = script;
            subIntConf.SetActive(true);
        }
        else
        {
            subIntConf.SetActive(false);
        }
    }


    public void optVis(string obj)
    {
        if (obj != null && optMenu.activeSelf == false)
        {
            optMenu.SetActive(true);
            referen = manager.FindByGuid(obj);
            Debug.LogWarning("fire");
        }
        else
        {
            optMenu.SetActive(false);
        }
    }

    public void optVisNoArgClearHold()
    {
        if (optMenu.activeSelf == false)
        {
            optMenu.SetActive(true);
        }
        else
        {
            optMenu.SetActive(false);
            referen = null;
        }
    }

    public void subSelcVis()
    {
        if (Subints.activeSelf)
            Subints.SetActive(false);
        else
            Subints.SetActive(true);
    }

    public string formName(int num)
    {
        return num.ToString().PadLeft(4, '0');
    }

    public string formSubIntName(string num)
    {
        return referen.name + $".{num}";
    }

    public void createVlan(int num)
    {
        vlanList rb = this.gameObject.AddComponent<vlanList>();
        rb.setName(num);
        lister.knownActive.Add(rb);
    }

    public void createSubIntwithCheck()
    {
        bool found = false;
        foreach (routerSubInter subs in referen.GetComponents<routerSubInter>())
        {
            if (subs.callName() == formSubIntName(subintcreatetext.text))
                found = true;
        }
        if (found)
        { }
        else
        {
            referen.GetComponent<routerPort>().createSubInt(subintcreatetext.text);
        }

    }

    public void createVlanwithCheck()
    {
        bool found = false;
        foreach (var vlaner in lister.knownActive)
        {
            if (vlaner.listname == formName(Convert.ToInt32(vlancreatenum.text)))
                found = true;
        }
        if (found)
        { }
        else
        {
            createVlan(Convert.ToInt32(vlancreatenum.text));
        }

    }

    public void destroyVlan(string vlanName)
    {
        List<vlanList> temp = new List<vlanList>();
        foreach (vlanList tempo in lister.knownActive)
        {
            temp.Add(tempo);
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

    public string returnAddress(string stringert)
    {
        Debug.LogWarning("routerProcess");
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<routerPort>(out routerPort porter) && porter.networkOut(stringert))
            {
                    return porter.getTarget(stringert);
            }
        }
        return null;
    }

    public bool VlanMapExists(string stringert, string vlanName)
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<routerPort>(out routerPort porter) && porter.vlanMapped(stringert, vlanName))
            {
                return true;
            }
        }
        return false;
    }


    public void confVisBack()
    {
        if (ipconf.activeSelf == false)
        {
            ipconf.SetActive(true);
            optMenu.SetActive(false);
        }
        else
        {
            ipconf.SetActive(false);
            ipaddress.text = null;
            mask.text = null;
        }
    }


    public void selcVis()
    {
        if (portselc != null && portselc.activeSelf == false)
            portselc.SetActive(true);
        else
            portselc.SetActive(false);
        
    }

    public void assignAddr()
    {
        if (referen.TryGetComponent<ipaddresser>(out ipaddresser addresser) &&
            ipaddress.text != "" &&
            mask.text != "")
        {
            addresser.setRawString(ipaddress.text);
            addresser.setMaskLength(Convert.ToInt32(mask.text));
            ipaddress.text = null;
            mask.text = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
