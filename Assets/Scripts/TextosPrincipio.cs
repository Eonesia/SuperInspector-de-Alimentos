using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class TextosPrincipio : MonoBehaviour
{
    public TextMeshProUGUI textoNarrativo;
    public float tiempoEntreFrases = 3f;

    private string[] frases = {
        "Trabajas en EoMarket como inspector de alimentos",
        "La sociedad hoy en dia valora mucho el etiquetado de los productos alimenticios",
        "Tu trabajo puede cambiar la vida de la gente",
        "Prepara las pegatinas y a calificar",
        "Tu jornada comienza ahora..."
    };

    void Start()
    {
        StartCoroutine(MostrarTextoNarrativo());
    }

    private IEnumerator MostrarTextoNarrativo()
    {
        foreach (string frase in frases)
        {
            // Establecer el texto con opacidad cero
            textoNarrativo.text = frase;
            textoNarrativo.alpha = 0;

            // Fade in
            yield return StartCoroutine(FadeText(0f, 1f, 1f));

            // Mantener visible
            yield return new WaitForSeconds(tiempoEntreFrases);

            // Fade out
            yield return StartCoroutine(FadeText(1f, 0f, 1f));
        }

        // Cambiar de escena al final
        SceneManager.LoadScene("EscenaPrueba"); // Cambia esto por el nombre de tu escena principal
    }

    private IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            textoNarrativo.alpha = alpha;
            elapsed += Time.deltaTime;
            yield return null;
        }

        textoNarrativo.alpha = endAlpha;
    }
}