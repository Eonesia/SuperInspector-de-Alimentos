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

    public int diaActual = 1;
    public int totalDias = 4;

    public GameObject pantallaResultado;
    public TextMeshProUGUI TextoResultado;
    public GameObject menuHUD;

    private void Start()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0, 0, 0, 0);
        transitionText.text = "";
        transitionText.alpha = 0;

        var canvasGroup = pantallaResultado.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
            canvasGroup.alpha = 0;
    }

    public void StartSceneTransition()
    {
        var sistemaPuntuacion = SistemaPuntuacion.instanciaUnica;

        if (sistemaPuntuacion == null)
        {
            Debug.LogError("❌ sistemaPuntuacion no está disponible.");
            return;
        }

        if (menuHUD != null && menuHUD.activeSelf)
        menuHUD.SetActive(false);

        if (diaActual >= totalDias)
        {
            sistemaPuntuacion.EvaluarResultadoFinal();

            if (pantallaResultado != null)
            {
                pantallaResultado.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                CanvasGroup grupo = pantallaResultado.GetComponent<CanvasGroup>();

                if (grupo != null)
                {
                    grupo.alpha = 0;
                    StartCoroutine(FadeCanvasGroup(grupo, 0f, 1f, 1.5f));
                }

                if (TextoResultado != null)
                {
                    if (sistemaPuntuacion.puntuacionTotal >= Mathf.Max(1, sistemaPuntuacion.puntuacionMinimaParaGanar))
                        TextoResultado.text = "Tu vida como inspector de alimentos continúa, eres un buen trabajador";
                    else
                        TextoResultado.text = "Las acciones que has llevado a cabo han hecho que seas despedido";
                }

                StartCoroutine(EsperarYIrAlMenu());
                return;
            }

            Debug.LogError("⚠️ pantallaResultado no está asignada.");
            return;
        }

        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        yield return StartCoroutine(FadeImage(0, 1));

        transitionText.text = message;
        yield return StartCoroutine(FadeText(0, 1));
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(FadeText(1, 0));

        diaActual++;

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogError("❌ No hay más escenas en el Build Settings.");
        }
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

    private IEnumerator FadeCanvasGroup(CanvasGroup group, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        group.alpha = startAlpha;

        while (elapsed < duration)
        {
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        group.alpha = endAlpha;
    }

    private IEnumerator EsperarYIrAlMenu()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MenuInicio");
    }
}



