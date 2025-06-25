using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour
{
    public SceneTransitionManagerTMP transitionManager;

    public void Interact()
    {
        if (transitionManager != null)
        {
            transitionManager.StartSceneTransition();
        }
    }
}

