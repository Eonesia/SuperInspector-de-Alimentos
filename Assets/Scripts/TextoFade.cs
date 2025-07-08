using UnityEngine;
using System.Collections;

public class TextoFade : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float duracionFade = 1.5f;

    public void FadeIn()
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        StartCoroutine(Fade(0, 1)); // <- ¡Esto es lo importante!
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(Fade(1, 0));
    }

    private IEnumerator Fade(float from, float to)
{
    float tiempo = 0f;
    canvasGroup.alpha = from;

    while (tiempo < duracionFade)
    {
        tiempo += Time.deltaTime;
        float alpha = Mathf.Lerp(from, to, tiempo / duracionFade);
        canvasGroup.alpha = alpha;
        yield return null;
    }

    canvasGroup.alpha = to;

    if (to == 0f)
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        gameObject.SetActive(false);
    }
    else
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}

    private void Awake()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
        }
}
}