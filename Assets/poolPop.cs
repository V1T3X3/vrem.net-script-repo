using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class poolPop : MonoBehaviour
{
    [SerializeField] private DhcpService server;
    [SerializeField] private VerticalLayoutGroup layout;
    [SerializeField] private TMP_FontAsset fontAsset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        popPools();
    }

    public void layoutClear()
    {
        foreach (Transform child in layout.transform)
        {
            Destroy(child.gameObject);
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

    public void popPools()
    {
        layoutClear();
        foreach (DhcpPool pool in server.pools)
        {
            GameObject objInstance = new GameObject("TextContainer");
            objInstance.transform.SetParent(layout.transform, false);
            Image background = objInstance.AddComponent<Image>();
            background.color = Color.white;
            objInstance.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 32);
            objInstance.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
            GameObject textObject = new GameObject("Text");
            textObject.transform.SetParent(objInstance.transform, false);
            TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
            text.text = $"Network: {BinaryToDottedDecimal(pool.networkBin)}/{pool.maskLength.ToString()}";
            text.font = fontAsset;
            text.alignment = TextAlignmentOptions.Center;
            text.enableAutoSizing = true;
            text.color = Color.black;
            RectTransform textRect = text.rectTransform;
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            textRect.anchoredPosition = Vector2.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
