using UnityEngine;

public class InicializadorSistemaPuntuacion : MonoBehaviour
{
    public GameObject panelMensajeRepetido;
    public GameObject panelTodosEvaluados;
    public MenuLista menuLista;

    private void Start()
    {
        if (SistemaPuntuacion.instanciaUnica != null)
        {
            var sistema = SistemaPuntuacion.instanciaUnica;

            sistema.panelMensajeRepetido = panelMensajeRepetido;
            sistema.panelTodosEvaluados = panelTodosEvaluados;
            sistema.menuLista = menuLista;

            Debug.Log("✅ Referencias asignadas correctamente al SistemaPuntuacion.");
        }
        else
        {
            Debug.LogWarning("⚠️ SistemaPuntuacion no está inicializado.");
        }
    }
}

