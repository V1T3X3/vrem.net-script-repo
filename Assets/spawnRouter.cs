using UnityEngine;
using Indiemount.Pooler;

public class spawnRouter : MonoBehaviour
{
    [SerializeField] Transform spawnLocation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void spawnObj(string objectName)
    {
        ObjectPooler.Instance.SpawnFromPool(objectName, spawnLocation.position, Quaternion.identity);
        
    }

    public void destroyObjsinScene()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("userObjs");
        foreach (GameObject objects in objs)
        {
            Destroy(objects, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
