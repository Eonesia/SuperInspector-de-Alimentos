using UnityEngine;
using System.Collections;

public class EncestarPapelera : MonoBehaviour
{
    [Header("Asignaciones")]
    public GameObject[] particleObjects;       // Lista de objetos con ParticleSystem
    public AudioClip sonidoDesdeFuera;         // Clip de sonido
    public AudioSource audioSource;            // AudioSource que reproducirá el sonido

    private bool yaActivado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (yaActivado) return;

        // Puedes filtrar por tag si lo deseas
        // if (!other.CompareTag("Player")) return;

        yaActivado = true;

        // Reproducir sonido
        if (audioSource != null && sonidoDesdeFuera != null)
        {
            audioSource.PlayOneShot(sonidoDesdeFuera);
        }

        // Activar y reproducir todas las partículas
        foreach (GameObject particleObj in particleObjects)
        {
            if (particleObj != null)
            {
                particleObj.SetActive(true);

                ParticleSystem ps = particleObj.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play();
                    StartCoroutine(DesactivarDespues(particleObj, ps.main.duration + ps.main.startLifetime.constantMax));
                }
            }
        }
    }

    private IEnumerator DesactivarDespues(GameObject obj, float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        if (obj != null)
        {
            obj.SetActive(false);
        }
    }
}