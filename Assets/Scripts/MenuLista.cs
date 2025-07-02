using UnityEngine;

public class MenuLista : MonoBehaviour
{
    public GameObject objetoMenuLista;
    public GameObject hud;
    public MenuInspeccion menuInspeccion; // Referencia al menú de inspección
    public MenuPausa menuPausa;           // NUEVO: Referencia al menú de pausa

    public bool lista = false;

    public void AlternarLista()
    {
        // Bloquear si el menú de inspección o el menú de pausa están activos
        if (!lista && (
            (menuInspeccion != null && menuInspeccion.inspeccion) ||
            (menuPausa != null && menuPausa.pausa)))
            return;

        lista = !lista;
        objetoMenuLista.SetActive(lista);
        hud.SetActive(!lista);
    }
}
