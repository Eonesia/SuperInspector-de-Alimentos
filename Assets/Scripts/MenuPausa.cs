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
    public MessageDisplayManager messageDisplayManager;

    public void AlternarPausa()
    {
        // Si estamos en ajustes, volver al menú de pausa
        if (menuAjustes != null && menuAjustes.menuAjustesUI.activeSelf)
        {
            VolverDesdeAjustes();
            return;
        }

        pausa = !pausa;
        objetoMenuPausa.SetActive(pausa);

        bool otrosMenusAbiertos =
            (menuInspeccion != null && menuInspeccion.inspeccion) ||
            (menuCC != null && menuCC.Cuaderno) ||
            (menuLista != null && menuLista.lista);

        hud.SetActive(!pausa && !otrosMenusAbiertos);

        // Manejo de tiempo y cursor dependiendo del estado general
        if (pausa)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (botonInicial != null && objetoMenuPausa.activeInHierarchy)
            {
                CoroutineRunner.RunCoroutine(SeleccionarConRetraso(botonInicial));
            }
        }
        else
        {
            if (messageDisplayManager != null && messageDisplayManager.IsMessageVisible())
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                if (messageDisplayManager.botonInicial != null && messageDisplayManager.messagePanel.activeInHierarchy)
                {
                    CoroutineRunner.RunCoroutine(SeleccionarConRetraso(messageDisplayManager.botonInicial));
                }
            }
            else if (!otrosMenusAbiertos)
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
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
            CoroutineRunner.RunCoroutine(SeleccionarConRetraso(botonInicial));
        }
    }

    private IEnumerator SeleccionarConRetraso(GameObject objeto)
    {
        yield return null; // espera 1 frame para que se actualice UI
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(objeto);
    }
}