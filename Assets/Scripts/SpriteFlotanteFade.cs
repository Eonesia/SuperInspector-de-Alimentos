using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFlotanteFade : MonoBehaviour
{
    public float duracionFade = 1f;

    private SpriteRenderer sr;
    private Coroutine fadeCoroutine;
    private bool jugadorDentro = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        // Asegúrate de empezar invisible si quieres eso
        Color c = sr.color;
        c.a = 0f;
        sr.color = c;
    }

    private void Update()
    {
        // Billboard orientado correctamente para sprites planos
        if (Camera.main != null)
        {
            Vector3 camPos = Camera.main.transform.position;
            Vector3 lookDir = transform.position - camPos;

            // Solo giramos en el eje Y para evitar rotación extraña
            lookDir.y = 0f;
            transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;

            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(Fade(0f, 1f));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;

            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(Fade(1f, 0f));
        }
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        Color baseColor = sr.color;

        while (t < duracionFade)
        {
            float alpha = Mathf.Lerp(from, to, t / duracionFade);
            sr.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            t += Time.deltaTime;
            yield return null;
        }

        sr.color = new Color(baseColor.r, baseColor.g, baseColor.b, to);
    }
}