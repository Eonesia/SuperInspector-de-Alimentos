using UnityEngine;

public class UIEstrellas : MonoBehaviour
{
    public void Calificar(int nota)
    {
        // Buscar al jugador
        PlayerInteract player = FindObjectOfType<PlayerInteract>();
        if (player == null)
        {
            Debug.LogWarning("No se encontró al jugador.");
            return;
        }

        // Obtener el alimento seleccionado
        ItemObject alimento = player.TomarAlimento();
        if (alimento == null)
        {
            Debug.LogWarning("No hay alimento seleccionado para evaluar.");
            return;
        }

        // Buscar el sistema de puntuación
        SistemaPuntuacion sistema = FindObjectOfType<SistemaPuntuacion>();
        if (sistema == null)
        {
            Debug.LogWarning("No se encontró el sistema de puntuación.");
            return;
        }

        // Evaluar el alimento
        sistema.EvaluarAlimento((DefaultObject)alimento, nota);

        Debug.Log($"Alimento evaluado: {alimento.name} con nota {nota}");
    }
}
