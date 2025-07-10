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

    [Header("Bot�n de pasar p�gina")]
    public Button botonPagina;
    public Sprite iconoPrimeraPagina;
    public Sprite iconoSegundaPagina;



    private void Start()
    {
        messagePanel.SetActive(false);
        if (messageImage != null)
            messageImage.gameObject.SetActive(false);
    }

    //private void Update()
    //{
    //    if (isVisible && canClose && Mouse.current.leftButton.wasPressedThisFrame)
    //    {
    //        // Solo cerrar si NO se ha hecho clic sobre un bot�n u otro UI
    //        if (!EventSystem.current.IsPointerOverGameObject())
    //        {
    //            HideMessage();
    //        }
    //    }
    //}


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

        MostrarPagina(0); // Siempre empieza en la primera p�gina
    }


    private System.Collections.IEnumerator EnableCloseAfterDelay()
    {
        yield return null; // Espera un frame
        canClose = true;
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
        yield return new WaitForSeconds(0.01f); // Espera 0.2 segundos antes de reactivar
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

        // Cambiar el icono del bot�n seg�n la p�gina
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









