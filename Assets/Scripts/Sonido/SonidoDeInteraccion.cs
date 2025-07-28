using UnityEngine;

public class InteractableSound : MonoBehaviour
{
    public AudioClip interactionSound;
    public AudioSource audioSource;

    public void PlayInteractionSound()
    {
        if (interactionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(interactionSound);
        }
    }
}

