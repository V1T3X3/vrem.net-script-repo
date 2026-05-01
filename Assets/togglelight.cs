using UnityEngine;

public class togglelight : MonoBehaviour
{
    [SerializeField] Light stat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

    }

    public void toggle()
    {

        if (stat.isActiveAndEnabled == true)
        {
            stat.enabled = false;
        }
        else
        {
            stat.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
    

    }
}
