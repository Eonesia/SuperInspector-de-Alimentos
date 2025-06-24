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

    [Header("Highlight Material")]
    public Material highlightMaterial;  // Material para aplicar cuando se apunta al objeto

    private GameObject lastHitObject = null;

    // Guardamos los materiales originales para cada renderer
    private Renderer[] lastRenderers = null;
    private Material[][] lastOriginalMaterials = null;

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

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, interactableLayer))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != lastHitObject)
            {
                RestoreLastObjectMaterial();

                ApplyHighlight(hitObject);

                lastHitObject = hitObject;
            }
        }
        else
        {
            RestoreLastObjectMaterial();
            lastHitObject = null;
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (lastHitObject != null)
        {
            IInteractable interactable = lastHitObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    private void ApplyHighlight(GameObject obj)
    {
        // Conseguimos todos los Renderers en obj y sus hijos
        lastRenderers = obj.GetComponentsInChildren<Renderer>();

        // Guardamos los materiales originales de cada renderer
        lastOriginalMaterials = new Material[lastRenderers.Length][];

        for (int i = 0; i < lastRenderers.Length; i++)
        {
            lastOriginalMaterials[i] = lastRenderers[i].materials;

            // Creamos array para asignar el material highlight en cada slot
            Material[] highlightMats = new Material[lastOriginalMaterials[i].Length];
            for (int j = 0; j < highlightMats.Length; j++)
            {
                highlightMats[j] = highlightMaterial;
            }

            lastRenderers[i].materials = highlightMats;
        }
    }

    private void RestoreLastObjectMaterial()
    {
        if (lastRenderers != null && lastOriginalMaterials != null)
        {
            for (int i = 0; i < lastRenderers.Length; i++)
            {
                if (lastRenderers[i] != null)
                {
                    lastRenderers[i].materials = lastOriginalMaterials[i];
                }
            }
        }

        lastRenderers = null;
        lastOriginalMaterials = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        Gizmos.DrawRay(transform.position, transform.forward * rayDistance);
    }
}
