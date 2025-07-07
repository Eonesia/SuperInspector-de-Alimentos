using UnityEngine;

public class MenuInspeccion : MonoBehaviour
{
    public GameObject objetoMenuInspeccion;
    public GameObject hud;
    public MenuLista menuLista;       // Referencia al menú de lista
    public MenuPausa menuPausa;       // Referencia al menú de pausa

    public InspectionHandler inspectionHandler; // Referencia al sistema de inspección
    public PlayerInteract playerInteract;       // Referencia al sistema de inventario

    public bool inspeccion = false;

    public void AlternarInspeccion()
    {
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
            {
                playerInteract.ForzarObjetoInspeccionable();
                inspectionHandler.ActivarModoInspeccion(playerInteract.GetObjetosRecogidos());
            }
            else
            {
                inspectionHandler.DesactivarModoInspeccion();
            }
        }

    }

}

