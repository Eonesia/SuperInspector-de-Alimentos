using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TriggerCambioDeEscena : MonoBehaviour
{
    public string nombreEscenaDestino = "MenuInicio";

    // Acción pública para asignar desde otro script o inspector
    public InputAction clickAction;

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
                SceneManager.LoadScene(nombreEscenaDestino);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 1f;
            }
        }
    }
}
