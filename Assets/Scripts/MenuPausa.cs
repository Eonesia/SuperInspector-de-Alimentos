using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuPausa : MonoBehaviour
{
    public GameObject objetoMenuPausa;
    public GameObject hud;
    public GameObject botonInicial;
    public bool pausa = false;

    public MenuAjustes menuAjustes;
    public MenuInspeccion menuInspeccion;
    public MenuCC menuCC;
    public MenuLista menuLista;

    private MessageDisplayManager messageDisplayManager;

    private void Awake()
    {
        messageDisplayManager = FindObjectOfType<MessageDisplayManager>();
    }

    public void AlternarPausa()
    {
        // Si estamos en ajustes, volver al menú de pausa
        if (menuAjustes != null && menuAjustes.menuAjustesUI.activeSelf)
        {
            VolverDesdeAjustes();
            return;
        }

        // No abrir menú pausa si hay mensaje abierto (y no está en pausa)
        if (MessageDisplayManager.MensajeAbierto && !pausa)
        {
            return;
        }

        pausa = !pausa;
        objetoMenuPausa.SetActive(pausa);

        bool otrosMenusAbiertos =
            (menuInspeccion != null && menuInspeccion.inspeccion) ||
            (menuCC != null && menuCC.Cuaderno) ||
            (menuLista != null && menuLista.lista);

        hud.SetActive(!pausa && !otrosMenusAbiertos);

        if (pausa)
        {
            // Pausar el juego al abrir el menú pausa
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Al cerrar el menú pausa...

            if (messageDisplayManager != null && messageDisplayManager.IsMessageVisible())
            {
                // Juego pausado porque hay mensaje
                Time.timeScale = 0f;
                messageDisplayManager.ForzarCursorVisible();
            }
            else if ((menuInspeccion != null && menuInspeccion.inspeccion) ||
                     (menuCC != null && menuCC.Cuaderno))
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if (menuLista != null && menuLista.lista)
            {
                // Si menulista abierto al cerrar pausa, despausar juego
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        if (pausa && botonInicial != null)
        {
            StartCoroutine(SeleccionarConRetraso(botonInicial));
        }
    }

    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicio");
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    public void AbrirAjustes()
    {
        if (menuAjustes != null)
        {
            objetoMenuPausa.SetActive(false);
            menuAjustes.AbrirMenuAjustes();
        }
    }

    public void VolverDesdeAjustes()
    {
        if (menuAjustes != null)
        {
            menuAjustes.CerrarMenuAjustes();
        }

        objetoMenuPausa.SetActive(true);
        pausa = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (botonInicial != null)
        {
            StartCoroutine(SeleccionarConRetraso(botonInicial));
        }
    }

    private IEnumerator SeleccionarConRetraso(GameObject objeto)
    {
        yield return null; // espera 1 frame para que se actualice UI
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(objeto);
    }
}