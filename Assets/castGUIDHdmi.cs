using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class castGUIDHdmi : MonoBehaviour
{
    private GameObject obj;
    public bool activeState = false;
    [SerializeField] private UniqueID machineId;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    //Transmits machine GUID to the cable
    public void transmitIdent()
    {
        activeState = true;
        XRSocketInteractor sock = GetComponent<XRSocketInteractor>();
        if (sock.hasSelection)
        {
            sock.GetOldestInteractableSelected().transform.gameObject.GetComponent<getSib>().castguid(machineId.GUID);
            obj = sock.GetOldestInteractableSelected().transform.gameObject;
        }
        Debug.Log("neighbor acquired");
        sock.interactionLayers = InteractionLayerMask.GetMask("hdmiin");
        obj.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("hdmiin");
    }
    //nullifies the GUID sent from the cable
    public void nullify()
    {
        activeState = false;
        obj.GetComponent<getSib>().nullify();
        obj.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("hdmiout");
        XRSocketInteractor sock = GetComponent<XRSocketInteractor>();
        sock.interactionLayers = InteractionLayerMask.GetMask("hdmiout");
        Debug.Log("addresses nullified");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
