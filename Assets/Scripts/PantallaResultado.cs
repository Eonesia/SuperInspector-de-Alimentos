using UnityEngine;
using TMPro;

public class PantallaResultado : MonoBehaviour
{
    public TextMeshProUGUI textoResultado;
    public int puntuacionMinimaParaGanar = 15; // O el valor que usas en tu sistema

    void Start()
    {
        int puntosObtenidos = SistemaPuntuacion.instanciaUnica.puntuacionTotal;

        if (puntosObtenidos >= puntuacionMinimaParaGanar)
        {
            textoResultado.text = "¡Has ganado el juego! ";
        }
        else
        {
            textoResultado.text = "Has perdido... ";
        }

    }
}
