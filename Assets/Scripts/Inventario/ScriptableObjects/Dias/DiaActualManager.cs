using UnityEngine;
using System.Collections.Generic;

public class DiaActualManager : MonoBehaviour
{
    public DiaDisponible diaConfigurado;

    public static DiaActualManager instancia;

    private void Awake()
    {
        instancia = this;
    }

    public List<DefaultObject> ObtenerAlimentosPermitidos()
    {
        return diaConfigurado != null ? diaConfigurado.alimentosPermitidos : new List<DefaultObject>();
    }
}
