using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TextoFade : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float duracionFade = 1f;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeCanvasGroup(0f, 1f));
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeCanvasGroup(1f, 0f));
    }

    private IEnumerator FadeCanvasGroup(float startAlpha, float endAlpha)
    {
        float tiempo = 0f;
        while (tiempo < duracionFade)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, tiempo / duracionFade);
            tiempo += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }
}