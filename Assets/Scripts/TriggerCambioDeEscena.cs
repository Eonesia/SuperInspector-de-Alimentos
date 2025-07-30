using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class TriggerCambioDeEscena : MonoBehaviour
{
    public string nombreEscenaDestino = "MenuInicio";
    public AudioSource audioSource;
    public AudioClip sonidoReloj;

    public InputAction clickAction;
    public float delayAntesDeCambiar = 1.5f; // ← Puedes ajustar este valor

    private Camera cam;

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

    private void OnClick(InputAction.CallbackContext context)
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform) // Si el clic fue sobre este objeto
            {
                audioSource.PlayOneShot(sonidoReloj);
                StartCoroutine(CambiarEscenaConDelay());
            }
        }
    }

    private IEnumerator CambiarEscenaConDelay()
    {
        yield return new WaitForSeconds(delayAntesDeCambiar);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;

        SceneManager.LoadScene(nombreEscenaDestino);
    }
}
