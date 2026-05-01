using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class portsConfSub : MonoBehaviour
{
    [SerializeField] private routerBV router;
    [SerializeField] private VerticalLayoutGroup layout;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        Debug.Log("fireSubsFill");
        layoutClear();
        getSubs();
        layoutClear();
        getSubs();
    }

    private void OnDisable()
    {
        Debug.Log("fireClearings");
        layoutClear();
    }

    public void layoutClear()
    {
        foreach (Transform child in layout.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void getSubs()
    {
        foreach (routerSubInter child in router.referen.GetComponents<routerSubInter>())
        {
            if (child != null)
            {
                GameObject button = Instantiate(TMP_DefaultControls.CreateButton(new TMP_DefaultControls.Resources()));
                button.transform.SetParent(layout.transform, false);
                button.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 12);
                button.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
                button.GetComponent<Button>().onClick.AddListener(() => router.subConfSelc(child));
                button.GetComponent<Button>().onClick.AddListener(() => router.subSelcVis());
                button.GetComponent<Button>().onClick.AddListener(() => router.updateVlanSelection());
                button.GetComponentInChildren<TMP_Text>().text = child.callName();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
