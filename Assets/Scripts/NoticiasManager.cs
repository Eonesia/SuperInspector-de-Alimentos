using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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

    private void Start()
    {
        messagePanel.SetActive(false);
        if (messageImage != null)
            messageImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isVisible && Mouse.current.leftButton.wasPressedThisFrame)
        {
            HideMessage();

        }
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

    public void ShowMessage(string message, Sprite optionalImage = null)
    {
        messageText.text = message;
        messagePanel.SetActive(true);
        isVisible = true;
        NoticiasTrigger.SetActive(false);

        if (playerControllerScript != null)
            playerControllerScript.bloquearRotacion = true;

        if (messageImage != null)
        {
            if (optionalImage != null)
            {
                messageImage.sprite = optionalImage;
                messageImage.gameObject.SetActive(true);
            }
            else
            {
                messageImage.gameObject.SetActive(false);
            }
        }
    }

    public void HideMessage()
    {
        messagePanel.SetActive(false);
        isVisible = false;

        if (playerControllerScript != null)
            playerControllerScript.bloquearRotacion = false;
    }

    public void ActiveTrigger()
    {
        if (isVisible = false)
        {
            NoticiasTrigger.SetActive(true);
        }
    }

    public bool IsMessageVisible()
    {
        return isVisible;
    }

}







