using UnityEngine;

public class UIEstrellas : MonoBehaviour
{
    public void Calificar(int nota)
    {
        // Buscar al jugador
        PlayerInteract player = FindObjectOfType<PlayerInteract>();
        if (player == null)
        {
            Debug.LogWarning("No se encontr� al jugador.");
            return;
        }

        // Obtener el alimento seleccionado
        ItemObject alimento = player.TomarAlimento();
        if (alimento == null)
        {
            Debug.LogWarning("No hay alimento seleccionado para evaluar.");
            return;
        }

        // Buscar el sistema de puntuaci�n
        SistemaPuntuacion sistema = FindObjectOfType<SistemaPuntuacion>();
        if (sistema == null)
        {
            Debug.LogWarning("No se encontr� el sistema de puntuaci�n.");
            return;
        }

        // Evaluar el alimento
        sistema.EvaluarAlimento((DefaultObject)alimento, nota);

        Debug.Log($"Alimento evaluado: {alimento.name} con nota {nota}");
    }
}
