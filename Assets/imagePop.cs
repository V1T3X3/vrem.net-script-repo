using System.IO;
using TMPro;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

public class imagePop : MonoBehaviour
{
    [SerializeField] private WebServer server;
    [SerializeField] private GridLayoutGroup layout;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        popImage();
    }
    public void layoutClear()
    {
        foreach (Transform child in layout.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void popImage()
    {
        layoutClear();
        foreach (string pathText in server.getAllPNGPaths())
        {

                GameObject button = Instantiate(TMP_DefaultControls.CreateButton(new TMP_DefaultControls.Resources()));
                button.transform.SetParent(layout.transform, false);
                button.GetComponent<RectTransform>().sizeDelta = new Vector2(90, 12);
                button.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
                button.GetComponent<Button>().onClick.AddListener(() => server.LoadImagePathFromLocal(pathText));
                button.GetComponentInChildren<TMP_Text>().text = Path.GetFileNameWithoutExtension(pathText);
                button.GetComponentInChildren<TMP_Text>().fontSize = 24;
                button.GetComponent<Button>().onClick.AddListener(() => popImage());
                if (pathText == server.getSite())
                    button.GetComponent<Button>().interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
