using UnityEngine;

public class SistemaPuntuacion : MonoBehaviour
{
    public int puntuacionTotal = 0;

    public void EvaluarAlimento(DefaultObject alimento, int notaJugador)
    {
        int diferencia = Mathf.Abs(notaJugador - alimento.calidadReal);
        int puntosGanados = Mathf.Max(0, 6 - diferencia); // 6 si acierta, 0 si se equivoca por 6

        puntuacionTotal += puntosGanados;

        Debug.Log($"Evaluaste '{alimento.name}' con un {notaJugador}. Calidad real: {alimento.calidadReal}. Puntos ganados: {puntosGanados}. Total: {puntuacionTotal}");
    }
}