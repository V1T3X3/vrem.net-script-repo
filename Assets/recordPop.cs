using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class recordPop : MonoBehaviour
{
    [SerializeField] private domainName server;
    [SerializeField] private VerticalLayoutGroup layout;
    [SerializeField] private TMP_FontAsset fontAsset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        popRecs();
    }

    public void layoutClear()
    {
        foreach (Transform child in layout.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void popRecs()
    {
        layoutClear();
        foreach (KeyValuePair<string, string> mapping in server.getMaps())
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
            text.text = $"{mapping.Key} -> {mapping.Value}";
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
