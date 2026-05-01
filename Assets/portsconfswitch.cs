using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class portsconfswitch : MonoBehaviour
{
    [SerializeField] private switchBV switchs;
    [SerializeField] private GridLayoutGroup layout;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        getPorts();
    }

    public void getPorts()
    {
        foreach (Transform child in switchs.transform)
        {
            if (child.TryGetComponent<castGUID>(out castGUID caster))
            {
                GameObject button = Instantiate(TMP_DefaultControls.CreateButton(new TMP_DefaultControls.Resources()));
                button.transform.SetParent(layout.transform, false);
                button.GetComponent<RectTransform>().sizeDelta = new Vector2(90, 12);
                button.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
                button.GetComponent<Button>().onClick.AddListener(() => switchs.confVis(child.gameObject));
                button.GetComponent<Button>().onClick.AddListener(() => switchs.selcVis());
                button.GetComponent<Button>().onClick.AddListener(() => switchs.updateVlanSelection());
                button.GetComponentInChildren<TMP_Text>().text = caster.name;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
