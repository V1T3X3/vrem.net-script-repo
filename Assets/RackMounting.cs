using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class RackMounting : MonoBehaviour
{
    private GameObject obj ;
    [SerializeField] XRSocketInteractor sock;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void changeLayer()
    {
        if (sock.hasSelection)
            obj = sock.GetOldestInteractableSelected().transform.gameObject;
        sock.interactionLayers = InteractionLayerMask.GetMask("LockedInRack");
        obj.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("LockedInRack");
        Debug.Log("Object Racked");
    }

    public void outOfRack()
    {
        obj.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("WaitingForRack");
        sock.interactionLayers = InteractionLayerMask.GetMask("WaitingForRack");
        Debug.Log("UnitAvailable");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
