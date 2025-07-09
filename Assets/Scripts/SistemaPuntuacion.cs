using UnityEngine;
using System.Collections.Generic;

public class SistemaPuntuacion : MonoBehaviour
{
    public static SistemaPuntuacion instanciaUnica;

    public int puntuacionTotal = 0;
    public int puntuacionMinimaParaGanar = 0;

    public GameObject panelMensajeRepetido; // Arrástralo en el Inspector

    private HashSet<DefaultObject> alimentosEvaluados = new HashSet<DefaultObject>();

    private void Awake()
    {
        if (instanciaUnica == null)
        {
            instanciaUnica = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EvaluarAlimento(DefaultObject alimento, int notaJugador)
    {
        // 🚫 Verificar si el alimento está permitido hoy
        List<DefaultObject> permitidos = DiaActualManager.instancia.ObtenerAlimentosPermitidos();

        if (!permitidos.Contains(alimento))
        {
            Debug.Log($"El alimento '{alimento.name}' no se puede evaluar en esta escena.");
            // Aquí puedes mostrar un panel visual si lo deseas
            return;
        }

        // 📛 Verificar si ya fue evaluado
        if (alimentosEvaluados.Contains(alimento))
        {
            MostrarMensajeAlimentoRepetido();
            return;
        }

        // ✅ Evaluación normal
        int diferencia = Mathf.Abs(notaJugador - alimento.calidadReal);
        int puntosGanados = Mathf.Max(0, 6 - diferencia);
        puntuacionTotal += puntosGanados;

        alimentosEvaluados.Add(alimento);

        Debug.Log($"Evaluaste '{alimento.name}' con un {notaJugador}. " +
                  $"Calidad real: {alimento.calidadReal}. Puntos ganados: {puntosGanados}. " +
                  $"Total acumulado: {puntuacionTotal}");
    }

    public void EvaluarResultadoFinal()
    {
        if (puntuacionTotal >= puntuacionMinimaParaGanar)
        {
            Debug.Log("¡Ganaste el juego! ");
        }
        else
        {
            Debug.Log("Perdiste el juego ");
        }
    }

    public void ReiniciarPuntuacion()
    {
        puntuacionTotal = 0;
        alimentosEvaluados.Clear();
    }

    private void MostrarMensajeAlimentoRepetido()
    {
        if (panelMensajeRepetido != null)
        {
            panelMensajeRepetido.SetActive(true);
            var fade = panelMensajeRepetido.GetComponent<TextoFade>();
            if (fade != null)
            {
                fade.FadeIn();
                Invoke(nameof(OcultarMensajeAlimentoRepetido), 2.5f);
            }
        }
    }

    private void OcultarMensajeAlimentoRepetido()
    {
        if (panelMensajeRepetido != null)
        {
            var fade = panelMensajeRepetido.GetComponent<TextoFade>();
            if (fade != null)
            {
                fade.FadeOut();
            }
        }
    }
}