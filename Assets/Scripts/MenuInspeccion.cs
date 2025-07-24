using UnityEngine;
using UnityEngine.EventSystems;

public class MenuInspeccion : MonoBehaviour
{
    public GameObject objetoMenuInspeccion;
    public GameObject hud;
    public MenuLista menuLista;       
    public MenuPausa menuPausa;       

    public InspectionHandler inspectionHandler;
    public PlayerInteract playerInteract;

    public bool inspeccion = false;

    // Botón inicial que se seleccionará cuando se abra el menú
    public GameObject botonInicial;

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

        // Seleccionamos el botón inicial para navegación con mando cuando el menú se active
        if (inspeccion && botonInicial != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(botonInicial);
        }
    }

    public void MostrarMensajeEvaluacionRepetida()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (SistemaPuntuacion.instanciaUnica != null)
        {
            SistemaPuntuacion.instanciaUnica.MostrarMensajeAlimentoRepetido();
        }
    }
}

