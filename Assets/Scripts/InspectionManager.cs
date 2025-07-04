using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionHandler : MonoBehaviour
{
    public Vector3 offsetInspeccionPosicion = new Vector3(0, 0.1f, 0.2f);
    public Vector3 offsetInspeccionRotacion = new Vector3(15f, 0f, 0f);
    public float duracionTransicion = 0.5f;

    private Dictionary<Transform, Vector3> posicionesOriginales = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Quaternion> rotacionesOriginales = new Dictionary<Transform, Quaternion>();
    private List<Transform> objetosInspeccionados = new List<Transform>();

    /// <summary>
    /// Activa el modo de inspección para los objetos analizables en la mano.
    /// </summary>
    /// <param name="objetosEnMano">Lista de objetos que el jugador tiene en la mano.</param>
    public void ActivarModoInspeccion(List<Transform> objetosEnMano)
    {
        Debug.Log("Activando inspección");

        foreach (Transform objeto in objetosEnMano)
        {
            if (EsObjetoAnalizable(objeto))
            {
                Debug.Log("Objeto analizable encontrado: " + objeto.name);

                objetosInspeccionados.Add(objeto);
                posicionesOriginales[objeto] = objeto.localPosition;
                rotacionesOriginales[objeto] = objeto.localRotation;

                Vector3 destinoPos = objeto.localPosition + offsetInspeccionPosicion;
                Quaternion destinoRot = objeto.localRotation * Quaternion.Euler(offsetInspeccionRotacion);

                StartCoroutine(MoverObjetoSuavemente(objeto, destinoPos, destinoRot, duracionTransicion));
            }
        }
    }

    /// <summary>
    /// Restaura los objetos inspeccionados a su posición y rotación original suavemente.
    /// </summary>
    public void DesactivarModoInspeccion()
    {
        foreach (Transform objeto in objetosInspeccionados)
        {
            if (posicionesOriginales.ContainsKey(objeto) && rotacionesOriginales.ContainsKey(objeto))
            {
                Vector3 posOriginal = posicionesOriginales[objeto];
                Quaternion rotOriginal = rotacionesOriginales[objeto];

                StartCoroutine(MoverObjetoSuavemente(objeto, posOriginal, rotOriginal, duracionTransicion));
            }
        }

        objetosInspeccionados.Clear();
        posicionesOriginales.Clear();
        rotacionesOriginales.Clear();
    }

    /// <summary>
    /// Determina si un objeto es analizable según su tipo en el ScriptableObject.
    /// </summary>
    private bool EsObjetoAnalizable(Transform objeto)
    {
        Item item = objeto.GetComponent<Item>();
        if (item == null) return false;

        return item.item.type == ItemType.AlimentoObjetivo;
    }

    /// <summary>
    /// Mueve un objeto suavemente a una posición y rotación dadas.
    /// </summary>
    private IEnumerator MoverObjetoSuavemente(Transform objeto, Vector3 destinoPos, Quaternion destinoRot, float duracion)
    {
        Vector3 inicioPos = objeto.localPosition;
        Quaternion inicioRot = objeto.localRotation;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            float t = tiempo / duracion;
            objeto.localPosition = Vector3.Lerp(inicioPos, destinoPos, t);
            objeto.localRotation = Quaternion.Slerp(inicioRot, destinoRot, t);

            tiempo += Time.unscaledDeltaTime;
            yield return null;
        }

        objeto.localPosition = destinoPos;
        objeto.localRotation = destinoRot;
    }
}


