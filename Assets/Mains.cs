using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Mains : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor caster;
    private GameObject connectedObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void getType()
    {
        if (caster.hasSelection && manager.FindByGuid(caster.GetOldestInteractableSelected().transform.gameObject.GetComponent<getSib>().getcast()).GetComponent<switchBV>() != null)
        {
            connectedObj = manager.FindByGuid(caster.GetOldestInteractableSelected().transform.gameObject.GetComponent<getSib>().getcast());
        }
        else {
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
