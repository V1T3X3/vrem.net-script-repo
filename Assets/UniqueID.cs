using UnityEngine;

public class UniqueID : MonoBehaviour
{
    private string guid;
    public string GUID => guid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        guid = System.Guid.NewGuid().ToString();
        manager.Register(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
