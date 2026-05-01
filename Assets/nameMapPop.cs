using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nameMapPop : MonoBehaviour
{
    [SerializeField] private domainName server;
    [SerializeField] private VerticalLayoutGroup layout;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void layoutClear()
    {
        foreach (Transform child in layout.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnEnable()
    {
        popMaps();
    }


    public void popMaps()
    {
        layoutClear();
        foreach (KeyValuePair<string, string> mapping in server.getMaps())
        {
            GameObject button = Instantiate(DefaultControls.CreateToggle(new DefaultControls.Resources()));
            button.transform.SetParent(layout.transform, false);
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(320, 32);
            button.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
            button.GetComponentInChildren<Text>().text = $"{mapping.Key} -> {mapping.Value}";
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
