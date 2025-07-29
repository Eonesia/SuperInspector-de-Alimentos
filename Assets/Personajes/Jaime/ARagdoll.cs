using UnityEngine;

public class ARagdoll : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Renderer[] targetRenderers; // Objetos a los que se les cambiar� el material
    [SerializeField] private Material ragdollMaterial;   // Material a aplicar cuando se active el ragdoll

    private Rigidbody[] rigidbodies;

    public AudioSource audioSource;
    public AudioClip interactionSound;

    void Start()
    {
        rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
        SetEnabled(false);
    }

    void SetEnabled(bool enabled)
    {
        bool isKinematic = !enabled;
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = isKinematic;
        }

        animator.enabled = !enabled;

        // Solo cambia los materiales si se activa el ragdoll
        if (enabled && ragdollMaterial != null)
        {
            foreach (Renderer renderer in targetRenderers)
            {
                if (renderer != null)
                {
                    renderer.material = ragdollMaterial;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("InteractLayer"))
        {
            SetEnabled(true);
            audioSource.PlayOneShot(interactionSound);
        }
        
    }
}
