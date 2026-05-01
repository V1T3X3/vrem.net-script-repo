using UnityEngine;

// Attach this to an empty GameObject or call from GameManager
public class faderStarter : MonoBehaviour
{
    public void Start() => CreateFader();

    private void CreateFader()
    {
        var faderObj = this.gameObject;
        var fader = faderObj.AddComponent<SceneFader>();
        fader.fadeIn();
    }

    public void fadeout()
    {
        var faderObj = this.gameObject;
        faderObj.SetActive(true);
        var fader = faderObj.GetComponent<SceneFader>();
        fader.fadeOut();
    }
}
