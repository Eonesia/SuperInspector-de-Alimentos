using UnityEngine;

public class PruebaLanzamiento : MonoBehaviour, IInteractable
{
    [SerializeField] private float fuerzaDisparo = 10f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
 
    }

    public void Interact()
    {
        if (rb != null)
        {
            rb.AddForce(Vector3.up * fuerzaDisparo, ForceMode.Impulse);
        }
    }
}
