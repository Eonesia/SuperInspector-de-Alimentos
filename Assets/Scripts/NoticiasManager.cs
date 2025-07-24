using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MessageDisplayManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject messagePanel;
    public TextMeshProUGUI messageText;
    public Image messageImage;
    public GameObject NoticiasTrigger;

    [Header("Control del jugador")]
    public FirstPersonController playerControllerScript;

    private bool isVisible = false;
    private bool canClose = false;
    public Pagina[] paginas;
    private int paginaActual = 0;

    [Header("Botón de pasar página")]
    public Button botonPagina;
    public Sprite iconoPrimeraPagina;
    public Sprite iconoSegundaPagina;

    // NUEVO: botón inicial para navegar con mando
    [Header("Navegación mando")]
    public GameObject botonInicial;

    private void Start()
    {
        messagePanel.SetActive(false);
        if (messageImage != null)
            messageImage.gameObject.SetActive(false);
    }

    public void ToggleMessage(string message, Sprite optionalImage = null)
    {
        if (isVisible)
        {
            HideMessage();
        }
        else
        {
            ShowMessage(message, optionalImage);
        }
    }

    public void ShowMessage(string message = "", Sprite optionalImage = null)
    {
        messagePanel.SetActive(true);
        isVisible = true;
        canClose = false;
        StartCoroutine(EnableCloseAfterDelay());

        NoticiasTrigger.SetActive(false);

        if (playerControllerScript != null)
            playerControllerScript.bloquearRotacion = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        MostrarPagina(0);

        // NUEVO: seleccionar botón inicial para navegación con mando
        if (botonInicial != null)
        {
            StartCoroutine(SeleccionarConRetraso(botonInicial));
        }
    }

    private System.Collections.IEnumerator EnableCloseAfterDelay()
    {
        yield return null;
        canClose = true;
    }

    private System.Collections.IEnumerator SeleccionarConRetraso(GameObject boton)
    {
        yield return null; // espera un frame para asegurar el SetActive
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(boton);
    }

    public void HideMessage()
    {
        messagePanel.SetActive(false);
        isVisible = false;
        canClose = false;

        if (playerControllerScript != null)
            playerControllerScript.bloquearRotacion = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(ReactivateTriggerAfterDelay());
    }

    public void ActiveTrigger()
    {
        if (!isVisible)
        {
            NoticiasTrigger.SetActive(true);
        }
    }

    public bool IsMessageVisible()
    {
        return isVisible;
    }

    private System.Collections.IEnumerator ReactivateTriggerAfterDelay()
    {
        yield return new WaitForSeconds(0.01f);
        if (NoticiasTrigger != null)
            NoticiasTrigger.SetActive(true);
    }

    public void MostrarPagina(int indice)
    {
        if (indice < 0 || indice >= paginas.Length) return;

        paginaActual = indice;
        messageText.text = paginas[indice].texto;

        if (messageImage != null)
        {
            if (paginas[indice].imagen != null)
            {
                messageImage.sprite = paginas[indice].imagen;
                messageImage.gameObject.SetActive(true);
            }
            else
            {
                messageImage.gameObject.SetActive(false);
            }
        }

        if (botonPagina != null && botonPagina.image != null)
        {
            botonPagina.image.sprite = (paginaActual == 0) ? iconoPrimeraPagina : iconoSegundaPagina;
        }
    }

    public void CambiarPagina()
    {
        int siguientePagina = (paginaActual + 1) % paginas.Length;
        MostrarPagina(siguientePagina);
    }
}









