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
    public MenuInspeccion menuInspeccion;
    public ItemObject itemGenerico; // Asignar el ScriptableObject genérico en el Inspector


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

            if (Physics.Raycast(ray, out hit, 3f))
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
                    if (rb != null) rb.isKinematic = true;

                    nuevoObjeto.SetParent(Mano);
                    nuevoObjeto.localPosition = item.posicionEnMano;
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
                    if (trigger) trigger.Interact();
                    var messageTrigger = hit.collider.GetComponent<MessageTrigger>();
                    if (messageTrigger)
                    {
                        messageTrigger.Interact();
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

        int intentos = 0;
        int nuevaIndex = objetoActivoIndex;

        do
        {
            nuevaIndex += scroll > 0 ? 1 : -1;

            if (nuevaIndex >= objetosRecogidos.Count) nuevaIndex = 0;
            if (nuevaIndex < 0) nuevaIndex = objetosRecogidos.Count - 1;

            intentos++;

            // Validar que el índice es válido y el objeto no es null
            if (nuevaIndex >= 0 && nuevaIndex < objetosRecogidos.Count && objetosRecogidos[nuevaIndex] != null)
            {
                Transform candidato = objetosRecogidos[nuevaIndex];

                // Si no estamos en inspección, aceptamos cualquier objeto
                if (menuInspeccion == null || !menuInspeccion.inspeccion || EsAlimentoObjetivo(candidato))
                {
                    objetoActivoIndex = nuevaIndex;
                    ActualizarObjetoActivo();
                    break;
                }
            }

            if (intentos > objetosRecogidos.Count)
            {
                Debug.Log("No hay objetos del tipo AlimentoObjetivo para inspeccionar.");
                puedeCambiar = true;
                yield break;
            }

        } while (true);

        yield return new WaitForSecondsRealtime(tiempoEntreCambios);
        puedeCambiar = true;
    }






    private bool EsAlimentoObjetivo(Transform objeto)
    {
        if (objeto == null) return false;

        Item item = objeto.GetComponent<Item>();
        if (item == null || item.item == null) return false;

        if (itemGenerico == null)
        {
            Debug.LogWarning("itemGenerico no está asignado en PlayerInteract.");
            return false;
        }

        return item.item.type == ItemType.AlimentoObjetivo && item.item != itemGenerico;
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
            foreach (var col in objeto.GetComponentsInChildren<Collider>())
                Physics.IgnoreCollision(col, jugadorCollider, true);

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
            Transform objeto = objetosRecogidos[i];
            if (objeto == null) continue;

            bool esActivo = (i == objetoActivoIndex);
            bool esInspeccionable = EsAlimentoObjetivo(objeto);

            // Mostrar solo si es el activo y es inspeccionable (cuando el menú está activo)
            bool mostrarVisualmente = esActivo && (!menuInspeccion?.inspeccion ?? true || esInspeccionable);

            foreach (var renderer in objeto.GetComponentsInChildren<MeshRenderer>(true))
            {
                if (renderer != null)
                    renderer.enabled = mostrarVisualmente;
            }

            foreach (var collider in objeto.GetComponentsInChildren<Collider>(true))
            {
                if (collider != null)
                    collider.enabled = false;
            }

            if (esActivo)
            {
                StartCoroutine(IgnorarColisionTemporal(objeto, 0.5f));

                if (ultimoObjetoSoltado != null)
                {
                    Collider[] collsNuevo = objeto.GetComponentsInChildren<Collider>(true);
                    Collider[] collsSoltado = ultimoObjetoSoltado.GetComponentsInChildren<Collider>(true);

                    foreach (var colNuevo in collsNuevo)
                        foreach (var colSoltado in collsSoltado)
                            if (colNuevo != null && colSoltado != null)
                                Physics.IgnoreCollision(colNuevo, colSoltado, true);

                    StartCoroutine(RestaurarColisionEntreObjetos(collsNuevo, collsSoltado, 0.5f));
                }

                if (inspectionHandler != null && menuInspeccion != null && menuInspeccion.inspeccion)
                {
                    for (int j = 0; j < objetosRecogidos.Count; j++)
                    {
                        if (j != i && objetosRecogidos[j] != null)
                        {
                            inspectionHandler.RestaurarInspeccionIndividual(objetosRecogidos[j]);
                        }
                    }

                    if (esInspeccionable)
                    {
                        inspectionHandler.AplicarAnimacionInspeccionIndividual(objeto);
                    }
                    else
                    {
                        inspectionHandler.RestaurarInspeccionIndividual(objeto);
                    }
                }
            }
        }
    }







    IEnumerator IgnorarColisionTemporal(Transform objeto, float duracion)
    {
        Collider jugadorCollider = GetComponent<Collider>();
        foreach (var col in objeto.GetComponentsInChildren<Collider>())
            Physics.IgnoreCollision(col, jugadorCollider, true);

        yield return new WaitForSeconds(duracion);

        foreach (var col in objeto.GetComponentsInChildren<Collider>())
            Physics.IgnoreCollision(col, jugadorCollider, false);
    }

    IEnumerator RestaurarColisionEntreObjetos(Collider[] collsA, Collider[] collsB, float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var colA in collsA)
            foreach (var colB in collsB)
                Physics.IgnoreCollision(colA, colB, false);
    }

    public List<Transform> GetObjetosRecogidos()
    {
        return objetosRecogidos;
    }

    public ItemObject TomarAlimento()
    {
        if (objetoActivoIndex >= 0 && objetoActivoIndex < objetosRecogidos.Count)
        {
            Transform objeto = objetosRecogidos[objetoActivoIndex];
            Item item = objeto.GetComponent<Item>();
            if (item != null)
            {
                return item.item;
            }
        }
        return null;
    }

    public void ForzarObjetoInspeccionable()
    {
        if (menuInspeccion != null && menuInspeccion.inspeccion)
        {
            if (objetoActivoIndex < 0 || objetoActivoIndex >= objetosRecogidos.Count || !EsAlimentoObjetivo(objetosRecogidos[objetoActivoIndex]))
            {
                for (int i = 0; i < objetosRecogidos.Count; i++)
                {
                    if (EsAlimentoObjetivo(objetosRecogidos[i]))
                    {
                        objetoActivoIndex = i;
                        ActualizarObjetoActivo();
                        return;
                    }
                }

                Debug.Log("No hay objetos del tipo AlimentoObjetivo al activar el menú de inspección.");
            }
            else
            {
                ActualizarObjetoActivo();
            }
        }
    }







    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
}








