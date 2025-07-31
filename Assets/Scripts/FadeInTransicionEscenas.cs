using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SceneFadeIn : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.5f;

    private void Start()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 1); // Empieza completamente negro
            StartCoroutine(FadeIn());
        }
        else
        {
            Debug.LogError("❌ No se ha asignado fadeImage en SceneFadeIn.");
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, 0f);
    }
}

