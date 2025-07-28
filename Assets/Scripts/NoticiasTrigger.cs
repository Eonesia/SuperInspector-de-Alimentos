using UnityEngine;

public class MessageTrigger : MonoBehaviour
{
    public MessageDisplayManager messageManager;
    [TextArea]
    public string messageToShow;
    public Sprite imageToShow;
    public AudioSource audioSource;
    public AudioClip interactionSound;

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
        audioSource.PlayOneShot(interactionSound);
    }

    public void AudioPeriodico()
    {
        audioSource.PlayOneShot(interactionSound);
    }
}




