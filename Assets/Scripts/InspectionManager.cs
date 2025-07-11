using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionHandler : MonoBehaviour
{
    public float duracionTransicion = 0.5f;

    private Dictionary<Transform, Vector3> posicionesOriginales = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Quaternion> rotacionesOriginales = new Dictionary<Transform, Quaternion>();
    private List<Transform> objetosInspeccionados = new List<Transform>();

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

                Vector3 destinoPos = ObtenerPosicionInspeccion(objeto);
                Quaternion destinoRot = ObtenerRotacionInspeccion(objeto);

                StartCoroutine(MoverObjetoSuavemente(objeto, destinoPos, destinoRot, duracionTransicion));
            }
        }
    }

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

    public void AplicarAnimacionInspeccionIndividual(Transform objeto)
    {
        if (!EsObjetoAnalizable(objeto)) return;

        posicionesOriginales[objeto] = objeto.localPosition;
        rotacionesOriginales[objeto] = objeto.localRotation;

        Vector3 destinoPos = ObtenerPosicionInspeccion(objeto);
        Quaternion destinoRot = ObtenerRotacionInspeccion(objeto);

        StartCoroutine(MoverObjetoSuavemente(objeto, destinoPos, destinoRot, duracionTransicion));
    }

    private Quaternion ObtenerRotacionInspeccion(Transform objeto)
    {
        Item item = objeto.GetComponent<Item>();
        if (item != null)
        {
            return Quaternion.Euler(item.rotacionInspeccionPersonalizada);
        }

        return objeto.localRotation;
    }

    private Vector3 ObtenerPosicionInspeccion(Transform objeto)
    {
        Item item = objeto.GetComponent<Item>();
        if (item != null)
        {
            return item.posicionInspeccionPersonalizada;
        }

        return objeto.localPosition;
    }

    private bool EsObjetoAnalizable(Transform objeto)
    {
        Item item = objeto.GetComponent<Item>();
        if (item == null) return false;

        return item.item.type == ItemType.AlimentoObjetivo;
    }

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

    public void RestaurarInspeccionIndividual(Transform objeto)
    {
        if (posicionesOriginales.ContainsKey(objeto) && rotacionesOriginales.ContainsKey(objeto))
        {
            Vector3 posOriginal = posicionesOriginales[objeto];
            Quaternion rotOriginal = rotacionesOriginales[objeto];

            StartCoroutine(MoverObjetoSuavemente(objeto, posOriginal, rotOriginal, duracionTransicion));
        }
    }
}





