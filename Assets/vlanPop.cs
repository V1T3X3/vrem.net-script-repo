using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UIElements.Image;

public class vlanPop : MonoBehaviour
{
    [SerializeField] private lister vlans;
    [SerializeField] private VerticalLayoutGroup layout;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void OnEnable()
    {
        popVlans();
    }

    public void layoutClear()
    {
        foreach (Transform child in layout.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void popVlans()
    {
        layoutClear();
        foreach (var namer in vlans.knownActive)
        {
            GameObject button = Instantiate(DefaultControls.CreateToggle(new DefaultControls.Resources()));
            button.transform.SetParent(layout.transform, false);
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 32);
            button.GetComponent<RectTransform>().localScale = new Vector3(0.32f, 0.32f, 0.32f);
            button.GetComponentInChildren<Text>().text = namer.listname;
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
