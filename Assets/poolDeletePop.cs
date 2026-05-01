using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class poolDeletePop : MonoBehaviour
{
    [SerializeField] private DhcpService server;
    [SerializeField] private VerticalLayoutGroup layout;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        popEntries();
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

    public void layoutClear()
    {
        foreach (Transform child in layout.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void popEntries()
    {
        layoutClear();
        foreach (DhcpPool mapping in server.pools)
        {
            GameObject button = Instantiate(DefaultControls.CreateToggle(new DefaultControls.Resources()));
            button.transform.SetParent(layout.transform, false);
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(320, 32);
            button.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
            button.GetComponentInChildren<Text>().text = $"{BinaryToDottedDecimal(mapping.networkBin)}/{mapping.maskLength.ToString()}";
            button.GetComponent<UnityEngine.UI.Toggle>().isOn = false;
            button.GetComponentInChildren<Text>().fontSize = 18;
            button.GetComponentInChildren<Text>().color = Color.white;
            foreach (UnityEngine.UI.Image imager in button.GetComponentsInChildren<UnityEngine.UI.Image>())
            {
                if (imager.gameObject.name == "Checkmark")
                {
                    imager.color = Color.black;
                    imager.sprite = Resources.Load<Sprite>("Sprites/Checkmark");
                }
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
