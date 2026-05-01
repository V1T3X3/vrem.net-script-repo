using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private float fadeInDuration = 3f;
    private CanvasGroup canvasGroup;
    private Image fadeImage;

    private void Awake()
    {

    }

    private void Start()
    {

    }

    public void fadeIn()
    {
        // Ensure this object persists until fade completes
        DontDestroyOnLoad(gameObject);

        // Set up canvas and image
        var canvas = gameObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay; // Works universally

        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        fadeImage = new GameObject("FadeImage").AddComponent<Image>();
        fadeImage.transform.SetParent(transform, false);
        fadeImage.color = Color.black;

        // Stretch to full screen
        var rect = fadeImage.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        // Start fully transparent
        canvasGroup.alpha = 1; // Start opaque for fade-in
        StartCoroutine(FadeIn());
    }

    public void fadeOut()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float timeElapsed = 0;
        while (timeElapsed < fadeInDuration)
        {
            timeElapsed += Time.deltaTime;
            canvasGroup.alpha = 1 - (timeElapsed / fadeInDuration);
            yield return null;
        }
        canvasGroup.alpha = 0; // Fully transparent
        gameObject.SetActive(false);
    }

    private IEnumerator FadeOut()
    {
        float timeElapsed = 0;
        fadeInDuration = 1.5f;
        while (timeElapsed < fadeInDuration)
        {
            timeElapsed += Time.deltaTime;
            canvasGroup.alpha = 0 + (timeElapsed / fadeInDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;  
    }
}