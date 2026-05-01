using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class manager : MonoBehaviour
{

    private static Dictionary<string, GameObject> guidToGameObject = new Dictionary<string, GameObject>();

    public static void Register(GameObject go)
    {
        if (go.TryGetComponent<UniqueID>(out var idComponent))
            guidToGameObject[idComponent.GUID] = go;
    }

    public static GameObject FindByGuid(string guid)
    {
        guidToGameObject.TryGetValue(guid, out GameObject go);
        return go;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
