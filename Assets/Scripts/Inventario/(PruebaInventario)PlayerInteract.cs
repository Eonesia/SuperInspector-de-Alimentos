using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteract : MonoBehaviour
{
    public InventoryObject inventory;
    public Transform Mano;
    public float fuerzaLanzamiento = 10f;
    public float ajusteLanzamientoIzq = 2f;
    public float ajusteLanzamientoDcha = 1.3f;
    public float tiempoEntreCambios = 0.3f;
    public float distanciaSoltarObjeto = 1.5f;
    public InputActionAsset inputActions;
    public InspectionHandler inspectionHandler;


    private List<Transform> objetosRecogidos = new List<Transform>();
    private int objetoActivoIndex = -1;
    private bool puedeCambiar = true;
    private Transform ultimoObjetoSoltado;

    private InputAction interactuarAction;
    private InputAction lanzarAction;
    private InputAction cambiarObjetoAction;
    private InputAction soltarAction;


    void Awake()
    {
        var mapa = inputActions.FindActionMap("Jugador");
        interactuarAction = mapa.FindAction("Interactuar");
        lanzarAction = mapa.FindAction("Lanzar");
        cambiarObjetoAction = mapa.FindAction("CambiarObjeto");
        soltarAction = mapa.FindAction("Soltar");
    }

    void OnEnable()
    {
        interactuarAction.Enable();
        lanzarAction.Enable();
        cambiarObjetoAction.Enable();
        soltarAction.Enable();
    }

    void OnDisable()
    {
        interactuarAction.Disable();
        lanzarAction.Disable();
        cambiarObjetoAction.Disable();
        soltarAction.Disable();
    }

    void Update()
    {
        if (interactuarAction.WasPressedThisFrame())
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            float distanciaMaximaRay = 3f;
            if (Physics.Raycast(ray, out hit, distanciaMaximaRay))
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
                    nuevoObjeto.localRotation = Quaternion.Euler(item.rotacionEnMano);

                    foreach (var col in nuevoObjeto.GetComponentsInChildren<Collider>())
                        col.enabled = false;

                    objetosRecogidos.Add(nuevoObjeto);
                    objetoActivoIndex = objetosRecogidos.Count - 1;

                    ActualizarObjetoActivo();
                }
                else
                {
                    var trigger = hit.collider.GetComponent<SceneChangeTrigger>();
                    if (trigger)
                    {
                        trigger.Interact();
                    }
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

        if (soltarAction.WasPressedThisFrame() && objetoActivoIndex >= 0)
        {
            SoltarObjeto();
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
        ItemObject itemData = objeto.GetComponent<Item>().item;

        Rigidbody rb = objeto.GetComponent<Rigidbody>();
        if (rb != null)
        {
            objeto.SetParent(null);
            rb.isKinematic = false;

            foreach (var col in objeto.GetComponentsInChildren<Collider>())
                col.enabled = true;

            rb.AddForce(Camera.main.transform.forward * fuerzaLanzamiento, ForceMode.Impulse);
            rb.AddForce(-Camera.main.transform.right * ajusteLanzamientoIzq, ForceMode.Impulse);
            rb.AddForce(Camera.main.transform.right * ajusteLanzamientoDcha, ForceMode.Impulse);
        }

        inventory.RemoveItem(itemData, 1);
        objetosRecogidos.RemoveAt(objetoActivoIndex);
        objetoActivoIndex = objetosRecogidos.Count > 0 ? 0 : -1;

        yield return new WaitForSeconds(0.2f);
        ActualizarObjetoActivo();
    }

    void SoltarObjeto()
    {
        Transform objeto = objetosRecogidos[objetoActivoIndex];
        ItemObject itemData = objeto.GetComponent<Item>().item;

        Rigidbody rb = objeto.GetComponent<Rigidbody>();
        if (rb != null)
        {
            objeto.SetParent(null);
            rb.isKinematic = false;
            rb.useGravity = true;

            foreach (var col in objeto.GetComponentsInChildren<Collider>())
                col.enabled = true;

            Collider jugadorCollider = GetComponent<Collider>();
            Collider[] colls = objeto.GetComponentsInChildren<Collider>();
            foreach (var col in colls)
            {
                Physics.IgnoreCollision(col, jugadorCollider, true);
            }

            Vector3 dropPosition = Camera.main.transform.position + Camera.main.transform.forward * distanciaSoltarObjeto;
            rb.MovePosition(dropPosition);

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        ultimoObjetoSoltado = objeto;

        inventory.RemoveItem(itemData, 1);
        objetosRecogidos.RemoveAt(objetoActivoIndex);
        objetoActivoIndex = objetosRecogidos.Count > 0 ? 0 : -1;

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
                collider.enabled = false;

            if (esActivo)
            {
                StartCoroutine(IgnorarColisionTemporal(objetosRecogidos[i], 0.5f));

                if (ultimoObjetoSoltado != null)
                {
                    Collider[] collsNuevo = objetosRecogidos[i].GetComponentsInChildren<Collider>();
                    Collider[] collsSoltado = ultimoObjetoSoltado.GetComponentsInChildren<Collider>();

                    foreach (var colNuevo in collsNuevo)
                    {
                        foreach (var colSoltado in collsSoltado)
                        {
                            Physics.IgnoreCollision(colNuevo, colSoltado, true);
                        }
                    }

                    StartCoroutine(RestaurarColisionEntreObjetos(collsNuevo, collsSoltado, 0.5f));
                }
            }
        }
    }

    IEnumerator IgnorarColisionTemporal(Transform objeto, float duracion)
    {
        Collider jugadorCollider = GetComponent<Collider>();
        Collider[] colls = objeto.GetComponentsInChildren<Collider>();

        foreach (var col in colls)
        {
            Physics.IgnoreCollision(col, jugadorCollider, true);
        }

        yield return new WaitForSeconds(duracion);

        foreach (var col in colls)
        {
            Physics.IgnoreCollision(col, jugadorCollider, false);
        }
    }

    IEnumerator RestaurarColisionEntreObjetos(Collider[] collsA, Collider[] collsB, float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var colA in collsA)
        {
            foreach (var colB in collsB)
            {
                Physics.IgnoreCollision(colA, colB, false);
            }
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }

    public List<Transform> GetObjetosRecogidos()
    {
        return objetosRecogidos;
    }
    public DefaultObject ObtenerAlimentoActivo()
    {
        if (objetoActivoIndex >= 0 && objetoActivoIndex < objetosRecogidos.Count)
        {
            Transform objeto = objetosRecogidos[objetoActivoIndex];
            Item itemComponent = objeto.GetComponent<Item>();
        if (itemComponent != null && itemComponent.item is DefaultObject alimento)
        {
            return alimento;
        }
    }
    return null;
    }

    public DefaultObject alimentoSeleccionado;

    public void TomarAlimento(DefaultObject alimento)
    {
        alimentoSeleccionado = alimento;
        Debug.Log($"Alimento seleccionado desde el inventario: {alimento.name}");
    }

    public DefaultObject GetAlimentoSeleccionado()
    {
        return alimentoSeleccionado;
    }
}






