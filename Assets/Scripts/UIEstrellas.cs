using UnityEngine;

public class UIEstrellas : MonoBehaviour
{
    public void Calificar(int nota)
    {
        PlayerInteract player = FindObjectOfType<PlayerInteract>();
        if (player != null)
        {
            DefaultObject alimento = player.GetAlimentoSeleccionado();
            if (alimento != null)
            {
                SistemaPuntuacion sistema = FindObjectOfType<SistemaPuntuacion>();
                if (sistema != null)
                {
                    sistema.EvaluarAlimento(alimento, nota);
                    Debug.Log($"Alimento evaluado: {alimento.name} con nota {nota}");
                }
            }
            else
            {
                Debug.LogWarning("No hay alimento seleccionado para evaluar.");
            }
        }
    }
}