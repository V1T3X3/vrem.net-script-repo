using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static System.Net.Mime.MediaTypeNames;

public class pcBV : MonoBehaviour, IResettable
{
    [SerializeField] private XRSocketInteractor sock;
    [SerializeField] private RawImage imager;
    public UniqueID machineId;
    [SerializeField] private togglelight machineLight;
    [SerializeField] private ipaddresser addresser;
    [SerializeField] private TMP_InputField ipaddress;
    [SerializeField] private TMP_InputField mask;
    [SerializeField] private TMP_InputField gateway;
    [SerializeField] private TMP_InputField dns;
    [SerializeField] private TMP_InputField target;
    [SerializeField] private TMP_InputField testerfield;
    [SerializeField] private GameObject UIGameObject;
    [SerializeField] private UnityEngine.UI.Toggle manualtoggle;
    [SerializeField] private UnityEngine.UI.Toggle dhcptoggle;
    [SerializeField] private UnityEngine.UI.Button setterbutton;
    [SerializeField] private TextMeshProUGUI cli;
    private string dhcp;
    private GameObject connectedObj;
    private float leasetime;
    private float leaseStartTime;
    private float leaseHalfTime;
    private bool contentor = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(renewLease), 0f, 1f);
        InvokeRepeating(nameof(gettem), 0f, 1f);
    }

    public void resetUI()
    {
        foreach (Transform child in UIGameObject.transform)
        {
            if (child.gameObject.name == "optScreen")
                child.gameObject.SetActive(true);
            else
                child.gameObject.SetActive(false);
        }

    }

    public void ResetToDefault()
    {
        manualtoggle.isOn = true; 
        dhcptoggle.isOn = false;
        imager.texture = null;
        addresser.clearInfo();
        setterbutton.interactable = true;
        ipaddress.text = "";
        ipaddress.interactable = true;
        mask.text = "";
        mask.interactable = true;
        gateway.text = "";
        gateway.interactable = true;
        dns.text = "";
        dns.interactable = true;
        target.text = "";
        testerfield.text = "";
        dhcp = "";
        connectedObj = null;
        leasetime = 0f;
        leaseStartTime = 0f;
        leaseHalfTime = 0f;
        contentor = true;
        resetUI();
    }

    public string returnDNS()
    {
        return dns.text;
    }

    public string returnLeaseTime()
    {
        return leasetime.ToString();
    }
    public string returnDHCP()
    {
        return dhcp;
    }
    public void gettem()
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

    public void setIp()
    {
        if (ipaddress.text != "" && mask.text != "")
        {
            addresser.setRawString(ipaddress.text);
            addresser.setMaskLength(Convert.ToInt32(mask.text));
            addresser.gateway = gateway.text;
        }
    }

    public void toggle()
    {
        machineLight.toggle();
    }

    public void LoadImageFromLocal(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            return;
        }
        Debug.LogWarning($"{filePath}");
        byte[] fileBytes = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileBytes);
        imager.texture = texture;
    }

    public string nslookup(string domainStatement)
    {
        manager.FindByGuid(findTarget(dns.text)).TryGetComponent<domainName>(out domainName namer);
        return namer.getMapping(domainStatement);
    }

    public void browser()
    {
        if (!Regex.IsMatch(target.text, @"[a-zA-Z]")) 
        { 
            manager.FindByGuid(findTarget(target.text)).TryGetComponent<WebServer>(out WebServer servicer);
            LoadImageFromLocal(servicer.getSite());
        }
        else
        {
            manager.FindByGuid(findTarget(nslookup(target.text))).TryGetComponent<WebServer>(out WebServer servicer);
            LoadImageFromLocal(servicer.getSite());
        }
        
    }


public string BinaryToDottedDecimal(string binary)
{
        /*
    if (string.IsNullOrEmpty(binary))
        throw new ArgumentException("Input cannot be null or empty.");

    if (binary.Length != 32)
        throw new ArgumentException("Input must be exactly 32 bits long.");

    if (!System.Text.RegularExpressions.Regex.IsMatch(binary, "^[01]{32}$"))
        throw new ArgumentException("Input must contain only 0s and 1s.");
        */
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
    public bool DhcpRequest()
    {
        if (dhcp != "" && manager.FindByGuid(findTarget(dhcp)).TryGetComponent<DhcpService>(out DhcpService servuicer))
        {
            return servuicer.acknowledgeRequest(addresser);
        }
        return false;
    }

    public void renewLease()
    {
        if (dhcp != "" && leaseStartTime != 0.0f && Time.time >= (leaseStartTime + leaseHalfTime))
        {
            try
            {
                manager.FindByGuid(findTarget(dhcp)).TryGetComponent<DhcpService>(out DhcpService servuicer);
                Debug.Log("refreshed address");
                servuicer.refreshLease(addresser);
                leaseStartTime = Time.time;
            }
            catch {
                if (dhcp != "" && leaseStartTime != 0.0f && Time.time >= (leaseStartTime + leasetime))
                {
                    Debug.Log("renewFailed");
                    ipaddress.text = string.Empty;
                    mask.text = string.Empty;
                    dns.text = string.Empty;
                    gateway.text = string.Empty;
                    leasetime = 0f;
                    leaseHalfTime = 0f;
                    leaseStartTime = 0f;
                    dhcp = "";
                    contentor = false;
                    addresser.clearInfo();
                }

            }
        }
    }
    System.Random rand = new System.Random();
    public async void tester()
    {
        Debug.Log(testerfield.text);
        string temp = null;
        try
        {
            temp = findTarget(testerfield.text);
            if (manager.FindByGuid(temp).TryGetComponent<pcBV>(out pcBV host))
            {
                await Task.Delay(rand.Next(2, 220));
                cli.text = "!";
                await Task.Delay(rand.Next(2, 220));
                cli.text += "!";
                await Task.Delay(rand.Next(2, 220));
                cli.text += "!";
                await Task.Delay(rand.Next(2, 220));
                cli.text += "! \n";
                cli.text += "Ping complete!";
                return;
            }
        }
        catch {
            await Task.Delay(rand.Next(2, 220));
            cli.text = ".";
            await Task.Delay(rand.Next(2, 220));
            cli.text += ".";
            await Task.Delay(rand.Next(2, 220));
            cli.text += ".";
            await Task.Delay(rand.Next(2, 220));
            cli.text += ". \n";
            cli.text += "Ping failed!";
            return;
        }
        
    }

    public void DhcpSetup()
    {
        if (connectedObj != null && connectedObj.TryGetComponent<switchBV>(out switchBV switchy) && contentor)
        {
            contentor = false;
            Debug.Log("fireTrigger");
            HashSet<string> keys = new HashSet<string>();
            string[] temp = switchy.findDhcp(addresser, machineId, keys).Split(" ");
            ipaddress.text = BinaryToDottedDecimal(temp[0]);
            mask.text = temp[1];
            dns.text = temp[2];
            gateway.text = temp[3];
            leasetime = (float.Parse(temp[4]) / 1000);
            leaseHalfTime = leasetime / 2;
            leaseStartTime = Time.time;
            dhcp = temp[5];
            setIp();
            try 
            {
                if (DhcpRequest())
                {
                    Debug.Log("Request Success");
                    return;
                }
            }
            catch
            {
                contentor = true;
                Debug.LogWarning("RequestFailed");
                ipaddress.text = string.Empty;
                mask.text = string.Empty;
                dns.text = string.Empty;
                gateway.text = string.Empty;
                leasetime = 0;
                leaseHalfTime = 0;
                leaseStartTime = 0;
                dhcp = "";
                addresser.clearInfo();
            }

        }

        else if (connectedObj != null && connectedObj.TryGetComponent<routerPort>(out routerPort parter) && contentor)
        {
            contentor = false;
            if (parter.helperExists())
            {
                string[] temp0 = parter.dhcpSearch().Split(" ");
                ipaddress.text = BinaryToDottedDecimal(temp0[0]);
                mask.text = temp0[1];
                dns.text = temp0[2];
                gateway.text = temp0[3];
                leasetime = (float.Parse(temp0[4]) / 1000);
                leaseHalfTime = leasetime / 2;
                leaseStartTime = Time.time;
                dhcp = temp0[5];
                setIp();
                if (DhcpRequest())
                {
                    Debug.Log("Request Success");
                    return;
                }
                else
                {
                    contentor = true;
                    Debug.Log("RequestFailed");
                    ipaddress.text = string.Empty;
                    mask.text = string.Empty;
                    dns.text = string.Empty;
                    gateway.text = string.Empty;
                    leasetime = 0;
                    leaseHalfTime = 0;
                    leaseStartTime = 0;
                    dhcp = "";
                    addresser.clearInfo();
                }

            }

        }

        else {
            contentor = true;
            Debug.Log("fireSafety");
            ipaddress.text = string.Empty;
            mask.text = string.Empty;
            dns.text = string.Empty;
            gateway.text = string.Empty;
            leasetime = 0;
            leaseHalfTime = 0;
            leaseStartTime = 0;
            dhcp = "";
            addresser.clearInfo();
        }
    }


    public string findTarget(string pivot)
    {
        if (isTargetOthNet(pivot) && addresser.gateway != "")
        {
            Debug.LogWarning("OthNetFired");
            if (connectedObj.TryGetComponent<switchBV>(out switchBV lista))
            {
                Debug.LogWarning("OthNetSwitchFired");
                HashSet<string> keys = new HashSet<string>();
                string temp = lista.getTarget(addresser, addresser.gateway, machineId, keys);
                Debug.LogWarning(temp);
                if (manager.FindByGuid(temp).TryGetComponent<routerPort>(out routerPort rote) && rote.relaytarget(pivot) != null)
                {
                    Debug.LogWarning("relayfoundtarget");
                    return rote.relaytarget(pivot);
                }
            }
            else if (connectedObj.TryGetComponent<routerPort>(out routerPort rote) && 
                connectedObj.TryGetComponent<ipaddresser>(out ipaddresser routaddresser) &&
                routaddresser.targetable(addresser, addresser.gateway))
            {
                Debug.LogWarning("OthNetDirectFired");
                return rote.relaytarget(pivot);
            }
        }


        else if (connectedObj.TryGetComponent<switchBV>(out switchBV lista) && !isTargetOthNet(pivot))
        {
            HashSet<string> keyser = new HashSet<string>();
            string temp = lista.getTarget(addresser, pivot, machineId, keyser);
            if (manager.FindByGuid(temp).TryGetComponent<ipaddresser>(out ipaddresser host) && 
                host.targetable(addresser, pivot))
                return temp;
            else
                Debug.LogWarning("yea, I ain't seeing 'im chief");
        }


        Debug.LogWarning("All statements processed in Host");
        return null;
    }
    /*
    public void testVlan()
    {
        if (isTargetOthNet(target.text))
        {
            Debug.LogWarning("OthNetFired");
            if (connectedObj.TryGetComponent<switchBV>(out switchBV lista))
            {
                Debug.LogWarning("OthNetSwitchFired");
                HashSet<string> keys = new HashSet<string>();
                string temp = lista.getTarget(addresser, addresser.gateway, machineId, keys);
                Debug.LogWarning(temp);
                if (manager.FindByGuid(temp).TryGetComponent<routerPort>(out routerPort rote) && rote.relaytarget(target.text) != null &&
                    manager.FindByGuid(rote.relaytarget(target.text)).TryGetComponent<pcBV>(out pcBV hoster))
                {
                    Debug.LogWarning("relayfoundtarget");
                    hoster.toggle();
                }

            }
            else if (connectedObj.TryGetComponent<routerPort>(out routerPort rote) && rote.relaytarget(target.text) != null &&
                manager.FindByGuid(rote.relaytarget(target.text)).TryGetComponent<pcBV>(out pcBV hoster))
            {
                Debug.LogWarning("OthNetDirectFired");
                hoster.toggle();
            }
        }
        else if (connectedObj.TryGetComponent<switchBV>(out switchBV lista))
        {
            HashSet<string> keyser = new HashSet<string>();
            string temp = lista.getTarget(addresser, target.text, machineId, keyser);
            if (manager.FindByGuid(temp).TryGetComponent<ServerBV>(out ServerBV host) && host.reachable(addresser.getRawString(), machineId.GUID))
                host.returnImage();
            else
                Debug.LogWarning("yea, I ain't seeing 'im chief");

        }
        else if (connectedObj.TryGetComponent<pcBV>(out pcBV host) &&
            host.reachable(addresser.getRawString(), machineId.GUID))
        {
            Debug.LogWarning("DirectConnect");
            host.toggle();
        }
    }
    */
    public bool isTargetOthNet(string targetadd)
    {
        if (addresser.ConvMaskBin(targetadd, addresser.maskLength) != addresser.maskBin)
            return true;
        else 
            return false;
    }

    public bool reachable(string targetId, string targetidentity)
    {
        if (isTargetOthNet(targetId) && addresser.gateway != "") {
            Debug.LogWarning("OthNetFired");
            if (connectedObj.TryGetComponent<switchBV>(out switchBV lister))
            {
                Debug.LogWarning("OthNetSwitchFired");
                HashSet<string> keys = new HashSet<string>();
                string temp = lister.getTarget(addresser, addresser.gateway, machineId, keys);
                Debug.LogWarning(temp);
                if (manager.FindByGuid(temp).TryGetComponent<UniqueID>(out UniqueID host) && host.GUID == targetidentity)
                    return true;
            }
            else if (connectedObj.TryGetComponent<routerPort>(out routerPort rote) && rote.relaytarget(targetId) != null &&
                manager.FindByGuid(rote.relaytarget(targetId)).TryGetComponent<pcBV>(out pcBV hoster) && hoster.machineId.GUID == targetidentity)
            {
                return true;
            }
        }
        else if (connectedObj.TryGetComponent<switchBV>(out switchBV lista))
        {
            HashSet<string> keys = new HashSet<string>();
            string temp = lista.getTarget(addresser, targetId, machineId, keys);
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

    
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
