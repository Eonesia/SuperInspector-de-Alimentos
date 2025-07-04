using UnityEngine;

public class AlimentoInteractivo : MonoBehaviour
{
    public DefaultObject datosAlimento; // Asigna el ScriptableObject desde el inspector

    public void AsignarNota(int nota)
    {
        SistemaPuntuacion sistema = FindObjectOfType<SistemaPuntuacion>();
        if (sistema != null && datosAlimento != null)
        {
            sistema.EvaluarAlimento(datosAlimento, nota);
        }
    }
}
