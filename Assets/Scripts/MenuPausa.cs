using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuPausa : MonoBehaviour
{
    public GameObject objetoMenuPausa;
    public GameObject hud;
    public GameObject botonInicial;
    public bool pausa = false;
    public GameObject objetoMenuAjustes;

    // ✅ Referencias a otros menús
    public MenuInspeccion menuInspeccion;
    public MenuCC menuCC;
    public MenuLista menuLista; // ✅ NUEVO: referencia al menú de lista

    public void AlternarPausa()
    {
        // ✅ Si el menú de ajustes está activo, ciérralo primero en lugar de cerrar el menú de pausa
        if (objetoMenuAjustes.activeSelf)
        {
            VolverDesdeAjustes();
            return;
        }

        pausa = !pausa;
        objetoMenuPausa.SetActive(pausa);

        // ✅ Verificar si hay otros menús abiertos antes de activar el HUD
        bool otrosMenusAbiertos =
            (menuInspeccion != null && menuInspeccion.inspeccion) ||
            (menuCC != null && menuCC.Cuaderno) ||
            (menuLista != null && menuLista.lista);

        hud.SetActive(!pausa && !otrosMenusAbiertos);

        Time.timeScale = pausa ? 0f : 1f;
        Cursor.lockState = pausa ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = pausa;

        if (pausa && botonInicial != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(botonInicial);
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
        objetoMenuPausa.SetActive(false);
        objetoMenuAjustes.SetActive(true);
    }

    public void VolverDesdeAjustes()
    {
        objetoMenuAjustes.SetActive(false);
        objetoMenuPausa.SetActive(true);

        if (botonInicial != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(botonInicial);
        }
    }
}