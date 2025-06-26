using UnityEngine;

public class MenuLista : MonoBehaviour
{
    public GameObject objetoMenuLista;
    public GameObject hud;

    public bool lista = false;

    public void AlternarLista()
    {
        lista = !lista;
        objetoMenuLista.SetActive(lista);
        hud.SetActive(!lista);

    }
}
