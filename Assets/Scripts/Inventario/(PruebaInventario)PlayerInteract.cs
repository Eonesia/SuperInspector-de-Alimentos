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

        // Lanzar objeto activo con clic derecho
        if (Input.GetMouseButtonDown(1) && objetoActivoIndex >= 0)
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
            ActualizarObjetoActivo();
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

    void ActualizarObjetoActivo()
    {
        for (int i = 0; i < objetosRecogidos.Count; i++)
        {
            foreach (var renderer in objetosRecogidos[i].GetComponentsInChildren<MeshRenderer>())
            {
                renderer.enabled = (i == objetoActivoIndex);
            }
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
}




