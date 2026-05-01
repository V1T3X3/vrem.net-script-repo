using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class portsconf : MonoBehaviour
{
    [SerializeField] private routerBV router;
    [SerializeField] private VerticalLayoutGroup layout;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        getPorts();
    }

    public void getPorts()
    {
        foreach (Transform child in router.transform)
        {
            if (child.TryGetComponent<castGUID>(out castGUID caster))
            {
                GameObject button = Instantiate(TMP_DefaultControls.CreateButton(new TMP_DefaultControls.Resources()));
                button.transform.SetParent(layout.transform, false);
                button.GetComponent<RectTransform>().sizeDelta = new Vector2 (90, 12);
                button.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
                button.GetComponent<Button>().onClick.AddListener(() => router.optVis(child.GetComponent<UniqueID>().GUID));
                button.GetComponent<Button>().onClick.AddListener(() => router.selcVis());
                button.GetComponentInChildren<TMP_Text>().text = caster.name;
            }
        }  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
