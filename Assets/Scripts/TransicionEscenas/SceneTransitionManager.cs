using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SceneTransitionManagerTMP : MonoBehaviour
{
    public Image fadeImage;
    public TextMeshProUGUI transitionText;
    public float fadeDuration = 1.5f;
    public float textFadeDuration = 1f;
    public string message = "Día siguiente...";
    public string sceneToLoad = "EscenaPrueba2";

    private void Start()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0, 0, 0, 0);
        transitionText.text = "";
        transitionText.alpha = 0;
    }

    public void StartSceneTransition()
    {
        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        // Fade out pantalla
        yield return StartCoroutine(FadeImage(0, 1));

        // Mostrar texto con fade in
        transitionText.text = message;
        yield return StartCoroutine(FadeText(0, 1));

        // Esperar un momento con el texto visible
        yield return new WaitForSeconds(1.5f);

        // Fade out del texto
        yield return StartCoroutine(FadeText(1, 0));

        // Cambiar de escena
        SceneManager.LoadScene(sceneToLoad);
    }

    private IEnumerator FadeImage(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
    }

    private IEnumerator FadeText(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;

        while (elapsed < textFadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / textFadeDuration);
            transitionText.alpha = alpha;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transitionText.alpha = endAlpha;
    }
}

