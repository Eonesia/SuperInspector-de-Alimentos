using UnityEngine;

public class RandomActivatorUI : MonoBehaviour
{
    public GameObject[] objetos;
    private int objetoActual = -1;

    void Awake()
    {
        DesactivarTodos();
    }

    void Start()
    {
        ActivarAleatorio();
    }

    public void CambiarObjetoDesdeBoton()
    {
        DesactivarTodos();
        ActivarAleatorio();
    }

    void DesactivarTodos()
    {
        for (int i = 0; i < objetos.Length; i++)
        {
            if (objetos[i] != null)
                objetos[i].SetActive(false);
        }
    }

    void ActivarAleatorio()
    {
        if (objetos.Length == 0) return;

        int nuevoIndice = objetoActual;

        if (objetos.Length > 1)
        {
            while (nuevoIndice == objetoActual)
                nuevoIndice = Random.Range(0, objetos.Length);
        }
        else
        {
            nuevoIndice = 0;
        }

        if (objetos[nuevoIndice] != null)
        {
            objetos[nuevoIndice].SetActive(true);
            objetoActual = nuevoIndice;
        }
    }
}
