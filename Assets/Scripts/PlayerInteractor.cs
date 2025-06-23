using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    public InventoryObject inventory;

    [Header("Input")]
    public InputActionReference interactInput;

    [Header("Raycast")]
    public float rayDistance = 5f;
    public LayerMask interactableLayer;

    [Header("Debug")]
    public Color rayColor = Color.green;

    private void OnEnable()
    {
        interactInput.action.Enable();
        interactInput.action.performed += OnInteract;
    }

    private void OnDisable()
    {
        interactInput.action.performed -= OnInteract;
        interactInput.action.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        
        Ray ray = new Ray(transform.position, transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, interactableLayer))
        {
            Debug.Log("Ray hit: " + hit.collider.name);
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        Gizmos.DrawRay(transform.position, transform.forward * rayDistance);
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
}
