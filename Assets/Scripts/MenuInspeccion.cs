using UnityEngine;

public class MenuInspeccion : MonoBehaviour
{
    public GameObject objetoMenuInspeccion;
    public GameObject hud;
    public MenuLista menuLista;       // Referencia al menú de lista
    public MenuPausa menuPausa;       // Referencia al menú de pausa

    public InspectionHandler inspectionHandler; // NUEVO: Referencia al sistema de inspección
    public PlayerInteract playerInteract;       // NUEVO: Referencia al sistema de inventario

    public bool inspeccion = false;

    public void AlternarInspeccion()
    {
        // Bloquear si el menú de lista o el menú de pausa están activos
        if (!inspeccion && (
            (menuLista != null && menuLista.lista) ||
            (menuPausa != null && menuPausa.pausa)))
            return;

        inspeccion = !inspeccion;
        objetoMenuInspeccion.SetActive(inspeccion);
        hud.SetActive(!inspeccion);

        Time.timeScale = inspeccion ? 0f : 1f;
        Cursor.lockState = inspeccion ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = inspeccion;

        if (inspectionHandler != null && playerInteract != null)
        {
            if (inspeccion)
                inspectionHandler.ActivarModoInspeccion(playerInteract.GetObjetosRecogidos());
            else
                inspectionHandler.DesactivarModoInspeccion();
        }
    }
}
