using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour
{
    public SceneTransitionManagerTMP transitionManager;
    public AudioSource audioSource;
    public AudioClip sonidoReloj;

    public void Interact()
    {
        if (transitionManager != null)
        {
            transitionManager.StartSceneTransition();
        }
        audioSource.PlayOneShot(sonidoReloj);
    }
}

