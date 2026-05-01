using UnityEngine;

public class quitter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void quitNow()
    {
        Invoke("quitApp", 2f);
    }

    private void quitApp()
    {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
