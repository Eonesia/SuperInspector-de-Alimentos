using UnityEngine;
using TMPro;
using System.Collections;

public class TextoFlotanteFade : MonoBehaviour
{
    public TextMeshPro texto3D;         // Asignar en el Inspector
    public float duracionFade = 1f;

    private bool mostrando = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!mostrando && other.CompareTag("Player"))
        {
            StartCoroutine(FadeInTexto());
        }
    }

    private IEnumerator FadeInTexto()
    {
        mostrando = true;
        float t = 0f;

        Color colorInicial = texto3D.color;
        colorInicial.a = 0f;
        texto3D.color = colorInicial;

        while (t < duracionFade)
        {
            float alpha = Mathf.Lerp(0f, 1f, t / duracionFade);
            texto3D.color = new Color(colorInicial.r, colorInicial.g, colorInicial.b, alpha);
            t += Time.deltaTime;
            yield return null;
        }

        texto3D.color = new Color(colorInicial.r, colorInicial.g, colorInicial.b, 1f);
    }
}
