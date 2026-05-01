using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
//sing static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class scanRack : MonoBehaviour
{
    [SerializeField] private GameObject units;
    [SerializeField] private VerticalLayoutGroup layout;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject ejectbutton;
    private GameObject tempObj;
    private Transform uiObj;
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

    public void listRack()
    {
        layoutClear();
        foreach (Transform child in units.transform)
        {
            if (child.TryGetComponent<XRSocketInteractor>(out XRSocketInteractor mounter) &&
                mounter.hasSelection &&
                mounter.GetOldestInteractableSelected().transform.gameObject.TryGetComponent<Outline>(out Outline liner))
            {
                GameObject button = Instantiate(TMP_DefaultControls.CreateButton(new TMP_DefaultControls.Resources()));
                button.transform.SetParent(layout.transform, false);
                button.GetComponent<RectTransform>().sizeDelta = new Vector2(20f, 2.5f);
                button.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
                button.GetComponentInChildren<TMP_Text>().text = mounter.GetOldestInteractableSelected().transform.gameObject.name;
                button.GetComponentInChildren<TMP_Text>().enableAutoSizing = true;
                button.GetComponentInChildren<TMP_Text>().fontSizeMin = 1f;
                button.transform.Rotate(0, 180, 0);
                button.GetComponent<Button>().onClick.AddListener(() => shiftUI(mounter.GetOldestInteractableSelected().transform.gameObject));
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    liner.enabled = !liner.enabled;
                });
            }
        }
    }

    public void shiftUI(GameObject obj)
    {
        tempObj = obj;
        uiObj = obj.transform.Find("UI");
        uiObj.gameObject.SetActive(true);
        uiObj.transform.SetParent(layout.gameObject.transform.parent, false);
        uiObj.transform.Rotate(new Vector3(0, 180, 0));
        uiObj.localPosition = Vector3.zero;
        uiObj.transform.SetParent(layout.gameObject.transform.parent.transform.parent, true);
        uiObj.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
        panel.SetActive(false);
        ejectbutton.transform.SetParent(uiObj.transform, true);
        ejectbutton.SetActive(true);
    }

    public void ejectUI()
    {
        if (tempObj != null && tempObj.TryGetComponent<Outline>(out Outline outliner))
            outliner.enabled = !outliner.enabled;
        if (!panel.activeSelf)
            panel.SetActive(true);
        if (ejectbutton.activeSelf)
        {
            ejectbutton.transform.SetParent(uiObj.transform.parent, true);
            ejectbutton.SetActive(false);
        }
        if (tempObj != null)
        {
            uiObj.transform.SetParent(tempObj.transform, false);
            Transform tempor = tempObj.transform.Find("UI");
            tempor.localScale = new Vector3(1, 1, 1);
            tempor.Rotate(new Vector3(0, 180, 0));
            tempor.localPosition = Vector3.zero;
            tempor.gameObject.SetActive(false);
        }
        tempObj = null;
        uiObj = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
