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

    public int diaActual = 1;
    public int totalDias = 4;

    public SistemaPuntuacion sistemaPuntuacion; // Asignar en el Inspector

    public GameObject pantallaResultado;        // Panel final
    public TextMeshProUGUI TextoResultado;      // Texto de resultado

    private void Start()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0, 0, 0, 0);
        transitionText.text = "";
        transitionText.alpha = 0;

        // Si usas CanvasGroup en pantallaResultado, asegúrate de tener alpha en 0 al inicio
        var canvasGroup = pantallaResultado.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
            canvasGroup.alpha = 0;
    }

    public void StartSceneTransition()
    {
        if (sistemaPuntuacion == null)
        {
            Debug.LogError("❌ sistemaPuntuacion no está asignado.");
            return;
        }

        if (diaActual >= totalDias)
        {
            sistemaPuntuacion.EvaluarResultadoFinal();

            // Activar el panel final con fade
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
                else
                {
                    Debug.LogWarning("⚠️ CanvasGroup no encontrado en pantallaResultado.");
                }

                if (TextoResultado != null)
            {
                if (sistemaPuntuacion.puntuacionTotal >= Mathf.Max(1, sistemaPuntuacion.puntuacionMinimaParaGanar))
                    TextoResultado.text = "Tu vida como inspector de alimentos continua, eres un buen trabajador";
                else
                    TextoResultado.text = "Las acciones que has llevado a cabo han hecho que seas despedido";

                
            }
            else
            {
                Debug.LogError("⚠️ textoResultado no está asignado.");
            }

                StartCoroutine(EsperarYIrAlMenu()); // ← Añade esta línea al final
                return;
            }
            else
            {
                Debug.LogError("⚠️ pantallaResultado no está asignada.");
            }

            return; // Detener transición
        }

        // Día normal: transición con fade y cambio de escena
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
    yield return new WaitForSeconds(10f); // 🕒 Espera de 10 segundos

    SceneManager.LoadScene("MenuInicio"); // Cambia "MenuInicio" por el nombre exacto de tu menú
}
}

