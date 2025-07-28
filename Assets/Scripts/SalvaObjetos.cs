using UnityEngine;

public class SalvaObjetos : MonoBehaviour
{
    [Header("Destino al que se clona el objeto")]
    public Transform destino;

    [Header("Fuerza de lanzamiento a aplicar al clon")]
    public Vector3 fuerzaLanzamiento = new Vector3(0, 0, 10f);

    private void OnTriggerEnter(Collider other)
    {
        if (destino == null)
        {
            Debug.LogWarning("No se ha asignado un destino en el teletransportador.");
            return;
        }

        GameObject original = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;

        // Pequeño offset hacia arriba para evitar overlaps con el suelo
        Vector3 offset = Vector3.up * 0.1f;

        // Instanciar clon en el destino
        GameObject clon = Instantiate(original, destino.position + offset, destino.rotation);

        // Configurar físicas
        Rigidbody rb = clon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.linearDamping = 0f;
            rb.angularDamping = 0.05f;

            // Aplicar fuerza de lanzamiento al clon
            rb.AddForce(clon.transform.TransformDirection(fuerzaLanzamiento), ForceMode.Impulse);
        }

        // Activar colliders
        foreach (Collider col in clon.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
            col.isTrigger = false;
        }

        // Eliminar original
        Destroy(original);
    }
}
