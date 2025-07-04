using UnityEngine;

public class UIEstrellas : MonoBehaviour
{
    public AlimentoInteractivo alimentoActual;

    public void Calificar(int nota)
    {
        if (alimentoActual != null)
        {
            alimentoActual.AsignarNota(nota);
        }
    }
}
