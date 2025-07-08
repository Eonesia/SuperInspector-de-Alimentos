using UnityEngine;
using System.Collections.Generic;

public class SistemaPuntuacion : MonoBehaviour
{
    public static SistemaPuntuacion instanciaUnica;

    public int puntuacionTotal = 0;
    public int puntuacionMinimaParaGanar = 0;

    private HashSet<DefaultObject> alimentosEvaluados = new HashSet<DefaultObject>();

    private void Awake()
    {
        if (instanciaUnica == null)
        {
            instanciaUnica = this;
            DontDestroyOnLoad(gameObject); // Este objeto sobrevive entre escenas
        }
        else
        {
            Destroy(gameObject); // Evita duplicados si ya existe uno
        }
    }

    public void EvaluarAlimento(DefaultObject alimento, int notaJugador)
    {
        if (alimentosEvaluados.Contains(alimento))
        {
            Debug.LogWarning($"El alimento '{alimento.name}' ya fue evaluado.");
            return;
        }

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
            Debug.Log("¡Ganaste el juego! 🎉");
        }
        else
        {
            Debug.Log("Perdiste el juego 😢");
        }
    }

    public void ReiniciarPuntuacion()
    {
        puntuacionTotal = 0;
        alimentosEvaluados.Clear();
    }
}