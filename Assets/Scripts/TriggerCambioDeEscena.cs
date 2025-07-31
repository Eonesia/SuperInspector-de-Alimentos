using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class TriggerCambioDeEscena : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoReloj;
    public InputAction clickAction;
    public float delayAntesDeTransicion = 1.5f;

    private Camera cam;
    private SceneTransitionManagerTMP sceneTransitionManager;

    private void OnEnable()
    {
        clickAction.Enable();
        clickAction.performed += OnClick;
        cam = Camera.main;
    }

    private void OnDisable()
    {
        clickAction.performed -= OnClick;
        clickAction.Disable();
    }

    private void Start()
    {
        // Busca el objeto que tenga el SceneTransitionManagerTMP en la escena
        sceneTransitionManager = FindObjectOfType<SceneTransitionManagerTMP>();

        if (sceneTransitionManager == null)
        {
            Debug.LogError("❌ No se encontró SceneTransitionManagerTMP en la escena.");
        }
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform)
            {
                audioSource.PlayOneShot(sonidoReloj);
                StartCoroutine(IniciarTransicionConDelay());
            }
        }
    }

    private IEnumerator IniciarTransicionConDelay()
    {
        yield return new WaitForSeconds(delayAntesDeTransicion);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;

        if (sceneTransitionManager != null)
        {
            sceneTransitionManager.StartSceneTransition();
        }
    }
}

