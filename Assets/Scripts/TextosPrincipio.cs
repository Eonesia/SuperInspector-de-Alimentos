using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems; // ← Necesario para seleccionar el botón

public class TextosPrincipio : MonoBehaviour
{
    public TextMeshProUGUI textoNarrativo;
    public float tiempoEntreFrases = 3f;
    public Button botonSkip;

    private Coroutine secuenciaTexto;

    private string[] frases = {
        "Trabajas en EoMarket como inspector de alimentos",
        "La sociedad hoy en día valora mucho el etiquetado de los productos alimenticios",
        "Tu trabajo puede cambiar la vida de la gente",
        "Prepara las pegatinas y a calificar",
        "Tu jornada comienza ahora..."
    };

    void Start()
    {
        // Iniciar narrativa
        secuenciaTexto = StartCoroutine(MostrarTextoNarrativo());

        // Asignar función al botón
        botonSkip.onClick.AddListener(SaltarNarrativa);

        // Seleccionar el botón para mando
        StartCoroutine(SeleccionarBotonConRetraso());
    }

    private IEnumerator MostrarTextoNarrativo()
    {
        foreach (string frase in frases)
        {
            textoNarrativo.text = frase;
            textoNarrativo.alpha = 0;

            yield return StartCoroutine(FadeText(0f, 1f, 1f));
            yield return new WaitForSeconds(tiempoEntreFrases);
            yield return StartCoroutine(FadeText(1f, 0f, 1f));
        }

        CargarSiguienteEscena();
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

    public void SaltarNarrativa()
    {
        if (secuenciaTexto != null)
            StopCoroutine(secuenciaTexto);

        textoNarrativo.alpha = 0f;
        botonSkip.gameObject.SetActive(false);

        CargarSiguienteEscena();
    }

    private void CargarSiguienteEscena()
    {
        SceneManager.LoadScene("EscenaPrueba"); // Cambia el nombre si lo necesitas
    }

    private IEnumerator SeleccionarBotonConRetraso()
    {
        yield return null; // Esperar un frame para asegurar que todo está cargado
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botonSkip.gameObject);
    }
}