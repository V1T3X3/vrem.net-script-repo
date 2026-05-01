using TMPro;
using UnityEngine;

public class PCSummary : MonoBehaviour
{
    [SerializeField] private TMP_Text sumText;
    [SerializeField] private ipaddresser addresser;
    [SerializeField] private pcBV host;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        sumText.text = $"Host addressing Summary: \n" +
            $"IP address: {addresser.getRawString()} \n" +
            $"Subnet Mask Length: {addresser.maskLength} \n" +
            $"Gateway Address: {addresser.gateway} \n" +
            $"DNS Server Address: {host.returnDNS()} \n" +
            $"DHCP Server Address: {host.returnDHCP()} \n" +
            $"DHCP Lease Time (Seconds): {host.returnLeaseTime()}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
