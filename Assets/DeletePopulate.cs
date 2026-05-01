using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DeletePopulate : MonoBehaviour
{
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

    public void deletables()
    {
        layoutClear();
        GameObject[] tranner = GameObject.FindGameObjectsWithTag("userObjs");
        foreach (GameObject obj in tranner)
        {
            if (obj.TryGetComponent<Outline>(out Outline liner) && 
                obj.TryGetComponent<callForDestroy>(out callForDestroy caller))
            {
                GameObject button = Instantiate(DefaultControls.CreateToggle(new DefaultControls.Resources()));
                button.transform.SetParent(layout.transform, false);
                button.GetComponent<RectTransform>().sizeDelta = new Vector2(110, 32);
                button.GetComponent<RectTransform>().localScale = new Vector3(0.005f, 0.005f, 0.005f);
                button.GetComponentInChildren<Text>().text = obj.name;
                button.GetComponent<UnityEngine.UI.Toggle>().isOn = false;
                button.GetComponent<UnityEngine.UI.Toggle>().onValueChanged.AddListener(_ => 
                {
                    liner.enabled = !liner.enabled;
                });
                button.GetComponentInChildren<Text>().fontSize = 18;
                button.GetComponentInChildren<Text>().color = Color.black;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
