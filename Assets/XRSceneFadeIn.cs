using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.XR.Oculus;
using System.Collections;

public class XRSceneFadeIn : MonoBehaviour
{
    [SerializeField] private float fadeInDuration = 2f;

    private void Start()
    {
        // Set initial black screen
        SetColorScale(0f);

        // Wait for scene to finish loading before starting fade
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Now begin fade-in
        StartCoroutine(FadeInRoutine());

        // Unsubscribe to avoid multiple calls
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private IEnumerator FadeInRoutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            float alpha = elapsedTime / fadeInDuration;
            SetColorScale(alpha);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        SetColorScale(1f);
    }

    private void SetColorScale(float scale)
    {
        Vector4 colorScale = new Vector4(scale, scale, scale, scale);
        Unity.XR.Oculus.Utils.SetColorScaleAndOffset(colorScale, Vector4.zero);
    }
}