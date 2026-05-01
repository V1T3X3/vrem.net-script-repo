using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class scanHdmi : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private XRSocketInteractor sock;
    private GameObject tempObj;
    private Transform uiObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(shiftUI), 0f, 1.5f);
    }

    public void shiftUI()
    {
        if (sock.hasSelection && sock.GetOldestInteractableSelected().transform.gameObject.TryGetComponent<getSib>(out getSib varo) &&
            varo.getcast() != null && 
            (uiObj == null || tempObj == null))
        {
            Debug.LogWarning("getUI");
            tempObj = manager.FindByGuid(varo.getcast());
            uiObj = tempObj.transform.Find("UI");
            uiObj.gameObject.SetActive(true);
            uiObj.transform.SetParent(canvas.transform, false);
            uiObj.transform.Rotate(new Vector3(0, 180, 0));
            uiObj.localPosition = Vector3.zero;
            uiObj.transform.localScale = new Vector3(15f, 15f, 15f);
        }
        else if (sock.hasSelection && 
            sock.GetOldestInteractableSelected().transform.gameObject.TryGetComponent<getSib>(out getSib kairo) && 
            kairo.getcast() == null && 
            (uiObj != null || tempObj != null))
        {
            ejectUI();
        }
            
        
    }

    public void ejectUI()
    {
        Debug.LogWarning("jumpUI");
        uiObj.transform.SetParent(tempObj.transform, false);
            Transform tempor = tempObj.transform.Find("UI");
            tempor.localScale = new Vector3(1, 1, 1);
            tempor.Rotate(new Vector3(0, 180, 0));
            tempor.localPosition = Vector3.zero;
            tempor.gameObject.SetActive(false);
            tempObj = null;
            uiObj = null;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
