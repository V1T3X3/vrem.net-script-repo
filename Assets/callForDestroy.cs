using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class callForDestroy : MonoBehaviour
{
    [SerializeField] private XRInteractionManager interactionManager;
    [SerializeField] private Object clears;
    private IResettable resetComp;
    private XRGrabInteractable grabInteractable;
    private XRSocketInteractor[] socks;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resetComp = clears as IResettable;
        interactionManager = FindAnyObjectByType<XRInteractionManager>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        socks = GetComponentsInChildren<XRSocketInteractor>(true);
    }

    private void OnDisable()
    {
        Outline tempo = GetComponent<Outline>();
        tempo.enabled = false;
        try
        {
            if (grabInteractable != null && grabInteractable.isSelected)
            {
                var dagger = grabInteractable.GetOldestInteractorSelecting();
                interactionManager.SelectExit(dagger, grabInteractable);
            }
            foreach (var sock in socks)
            {
                if (sock.hasSelection)
                    interactionManager.SelectExit(sock, sock.GetOldestInteractableSelected());
            }
            resetComp.ResetToDefault();
            Debug.LogWarning("Disabled pool member invoked");
        }
        catch { }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
