using UnityEngine;

public class MessageTrigger : MonoBehaviour
{
    public MessageDisplayManager messageManager;
    [TextArea]
    public string messageToShow;
    public Sprite imageToShow;

    public void Interact()
    {
        if (messageManager != null)
        {
            //// Si el mensaje ya está visible, lo cerramos directamente
            //if (messageManager.IsMessageVisible())
            //{
            //    messageManager.HideMessage();
            //}
            //else
            //{
                messageManager.ShowMessage(messageToShow, imageToShow);
            //}
        }
    }

}




