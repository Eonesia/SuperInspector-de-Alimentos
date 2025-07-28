using UnityEngine;
using TMPro;
using System.Collections;

public class TextoFlotanteFade : MonoBehaviour
{
    public TextMeshPro texto3D;         // Asignar en el Inspector
    public float duracionFade = 1f;

    private bool mostrando = false;
    private Coroutine fadeCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeInTexto());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeOutTexto());
        }
    }

    private IEnumerator FadeInTexto()
    {
        mostrando = true;
        float t = 0f;

        Color color = texto3D.color;
        color.a = 0f;
        texto3D.color = color;

        while (t < duracionFade)
        {
            float alpha = Mathf.Lerp(0f, 1f, t / duracionFade);
            texto3D.color = new Color(color.r, color.g, color.b, alpha);
            t += Time.deltaTime;
            yield return null;
        }

        texto3D.color = new Color(color.r, color.g, color.b, 1f);
    }

    private IEnumerator FadeOutTexto()
    {
        mostrando = false;
        float t = 0f;

        Color color = texto3D.color;
        color.a = 1f;

        while (t < duracionFade)
        {
            float alpha = Mathf.Lerp(1f, 0f, t / duracionFade);
            texto3D.color = new Color(color.r, color.g, color.b, alpha);
            t += Time.deltaTime;
            yield return null;
        }

        texto3D.color = new Color(color.r, color.g, color.b, 0f);
    }
}
