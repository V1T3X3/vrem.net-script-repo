using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class getSib : MonoBehaviour
{
    [SerializeField] private GameObject sibling;
    private getSib siblingComp;
    private string guid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (sibling != null) 
                siblingComp = sibling.GetComponent<getSib>();
    }


    public string getcast()
    {
        return guid; 
    }


    public void setSibCast(string cast)
    {
        guid = cast;
    }

    public void destroyself()
    {
        Destroy(this.gameObject);
        Destroy(siblingComp.gameObject);
    }

    public void nullify()
    {
      
        if (siblingComp.getcast() != null)
            siblingComp.setSibCast(null);
    }

    public void castguid(string id)
    {
        siblingComp.setSibCast(id);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
