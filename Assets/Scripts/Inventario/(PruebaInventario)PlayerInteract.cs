using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteract : MonoBehaviour
{
    public InventoryObject inventory;
    public Transform Mano;
    public float fuerzaLanzamiento = 10f;
    public float tiempoEntreCambios = 0.3f;

    private List<Transform> objetosRecogidos = new List<Transform>();
    private int objetoActivoIndex = -1;
    private bool puedeCambiar = true;

    void Update()
    {
        // Recoger objeto con clic izquierdo
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var item = hit.collider.GetComponent<Item>();
                if (item)
                {
                    if (objetosRecogidos.Count >= 5)
                    {
                        Debug.Log("No puedes llevar más de 5 objetos.");
                        return;
                    }

                    inventory.AddItem(item.item, 1);
                    Transform nuevoObjeto = item.transform;

                    Rigidbody rb = nuevoObjeto.GetComponent<Rigidbody>();
                    if (rb != null)
                        rb.isKinematic = true;

                    nuevoObjeto.SetParent(Mano);
                    nuevoObjeto.localPosition = Vector3.zero;
                    nuevoObjeto.localRotation = Quaternion.identity;

                    objetosRecogidos.Add(nuevoObjeto);
                    objetoActivoIndex = objetosRecogidos.Count - 1;

                    ActualizarObjetoActivo();
                }
            }
        }

        // Cambiar objeto con rueda del ratón (con delay)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (puedeCambiar && scroll != 0 && objetosRecogidos.Count > 0)
        {
            StartCoroutine(CambiarObjetoConDelay(scroll));
        }

        // Lanzar objeto activo con clic derecho (con delay para activar el siguiente)
        if (Input.GetMouseButtonDown(1) && objetoActivoIndex >= 0)
        {
            StartCoroutine(LanzarObjetoConDelay());
        }
    }

    IEnumerator CambiarObjetoConDelay(float scroll)
    {
        puedeCambiar = false;

        objetoActivoIndex += scroll > 0 ? 1 : -1;
        if (objetoActivoIndex >= objetosRecogidos.Count) objetoActivoIndex = 0;
        if (objetoActivoIndex < 0) objetoActivoIndex = objetosRecogidos.Count - 1;

        ActualizarObjetoActivo();

        yield return new WaitForSeconds(tiempoEntreCambios);
        puedeCambiar = true;
    }

    IEnumerator LanzarObjetoConDelay()
    {
        Transform objeto = objetosRecogidos[objetoActivoIndex];
        Rigidbody rb = objeto.GetComponent<Rigidbody>();
        if (rb != null)
        {
            objeto.SetParent(null);
            rb.isKinematic = false;
            rb.AddForce(Camera.main.transform.forward * fuerzaLanzamiento, ForceMode.Impulse);
        }

        objetosRecogidos.RemoveAt(objetoActivoIndex);
        objetoActivoIndex = objetosRecogidos.Count > 0 ? 0 : -1;

        yield return new WaitForSeconds(0.2f); // Delay antes de activar el siguiente objeto

        ActualizarObjetoActivo();
    }

    void ActualizarObjetoActivo()
    {
        for (int i = 0; i < objetosRecogidos.Count; i++)
        {
            bool esActivo = (i == objetoActivoIndex);

            // Activar/desactivar renderers
            foreach (var renderer in objetosRecogidos[i].GetComponentsInChildren<MeshRenderer>())
            {
                renderer.enabled = esActivo;
            }

            // Activar/desactivar colliders
            foreach (var collider in objetosRecogidos[i].GetComponentsInChildren<Collider>())
            {
                collider.enabled = esActivo;
            }
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
}







