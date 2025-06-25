using UnityEngine;

public class ARagdoll : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Rigidbody[] rigidbodies;

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
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("InteractLayer"))
        {
            SetEnabled(true);
        }
    }
}
