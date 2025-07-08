using UnityEngine;

public class SistemaPuntuacion : MonoBehaviour
{
    public int puntuacionTotal = 0;
    public int puntuacionMinimaParaGanar = 60; // Pon el valor que tú quieras desde el Inspector

    public void EvaluarAlimento(DefaultObject alimento, int notaJugador)
    {
        int diferencia = Mathf.Abs(notaJugador - alimento.calidadReal);
        int puntosGanados = Mathf.Max(0, 6 - diferencia);

        puntuacionTotal += puntosGanados;

        Debug.Log($"Evaluaste '{alimento.name}' con un {notaJugador}. " +
                  $"Calidad real: {alimento.calidadReal}. " +
                  $"Puntos ganados: {puntosGanados}. " +
                  $"Total acumulado: {puntuacionTotal}");
    }

    public void EvaluarResultadoFinal()
    {
        if (puntuacionTotal >= puntuacionMinimaParaGanar)
        {
            Debug.Log("¡Ganaste el juego! ");
            // Aquí puedes mostrar una UI de victoria o cargar otra escena
        }
        else
        {
            Debug.Log("Perdiste el juego ");
            // Aquí puedes mostrar una UI de derrota o reiniciar el juego
        }
    }
}