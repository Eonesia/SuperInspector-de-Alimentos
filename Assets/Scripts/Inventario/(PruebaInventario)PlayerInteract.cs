using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteract : MonoBehaviour
{
    public InventoryObject inventory;
    public Transform Mano;
    public float fuerzaLanzamiento = 10f;
    public float tiempoEntreCambios = 0.3f;
    public InputActionAsset inputActions;

    private List<Transform> objetosRecogidos = new List<Transform>();
    private int objetoActivoIndex = -1;
    private bool puedeCambiar = true;

    private InputAction interactuarAction;
    private InputAction lanzarAction;
    private InputAction cambiarObjetoAction;

    void Awake()
    {
        var mapa = inputActions.FindActionMap("Jugador");
        interactuarAction = mapa.FindAction("Interactuar");
        lanzarAction = mapa.FindAction("Lanzar");
        cambiarObjetoAction = mapa.FindAction("CambiarObjeto");
    }

    void OnEnable()
    {
        interactuarAction.Enable();
        lanzarAction.Enable();
        cambiarObjetoAction.Enable();
    }

    void OnDisable()
    {
        interactuarAction.Disable();
        lanzarAction.Disable();
        cambiarObjetoAction.Disable();
    }

    void Update()
    {
        if (interactuarAction.WasPressedThisFrame())
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
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

        float scroll = cambiarObjetoAction.ReadValue<Vector2>().y;
        if (puedeCambiar && scroll != 0 && objetosRecogidos.Count > 0)
        {
            StartCoroutine(CambiarObjetoConDelay(scroll));
        }

        if (lanzarAction.WasPressedThisFrame() && objetoActivoIndex >= 0)
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

        yield return new WaitForSeconds(0.2f);
        ActualizarObjetoActivo();
    }

    void ActualizarObjetoActivo()
    {
        for (int i = 0; i < objetosRecogidos.Count; i++)
        {
            bool esActivo = (i == objetoActivoIndex);

            foreach (var renderer in objetosRecogidos[i].GetComponentsInChildren<MeshRenderer>())
                renderer.enabled = esActivo;

            foreach (var collider in objetosRecogidos[i].GetComponentsInChildren<Collider>())
                collider.enabled = esActivo;
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
}








