using System.Collections;
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
    public Material highlightMaterial;

    [Header("Delay Settings")]
    public float restoreDelay = 1f;  // Tiempo en segundos para restaurar el material

    private GameObject lastHitObject = null;

    private Renderer[] lastRenderers = null;
    private Material[][] lastOriginalMaterials = null;

    private Coroutine restoreCoroutine = null;

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
                if (restoreCoroutine != null)
                {
                    StopCoroutine(restoreCoroutine);
                    restoreCoroutine = null;
                }

                RestoreLastObjectMaterialInstantly();

                ApplyHighlight(hitObject);

                lastHitObject = hitObject;
            }
        }
        else
        {
            if (lastHitObject != null)
            {
                if (restoreCoroutine == null)
                {
                    restoreCoroutine = StartCoroutine(RestoreMaterialWithDelay(restoreDelay));
                }
                lastHitObject = null;
            }
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
        lastRenderers = obj.GetComponentsInChildren<Renderer>();
        lastOriginalMaterials = new Material[lastRenderers.Length][];

        for (int i = 0; i < lastRenderers.Length; i++)
        {
            Renderer renderer = lastRenderers[i];
            Material[] originalMats = renderer.materials;
            lastOriginalMaterials[i] = originalMats;

            // Crear nuevo array con espacio para el outline
            Material[] newMats;
            if (originalMats.Length == 1)
            {
                newMats = new Material[2];
                newMats[0] = originalMats[0]; // material original
                newMats[1] = highlightMaterial; // outline en el slot 1
            }
            else
            {
                newMats = new Material[originalMats.Length];
                originalMats.CopyTo(newMats, 0);

                // Solo reemplazar o a�adir outline en slot 1 si es posible
                if (newMats.Length > 1)
                    newMats[1] = highlightMaterial;
            }

            renderer.materials = newMats;
        }
    }

    private void RestoreLastObjectMaterialInstantly()
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

    private IEnumerator RestoreMaterialWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        RestoreLastObjectMaterialInstantly();

        restoreCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        Gizmos.DrawRay(transform.position, transform.forward * rayDistance);
    }
}
