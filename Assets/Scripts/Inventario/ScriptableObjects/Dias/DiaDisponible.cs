using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DiaDisponible", menuName = "Configuracion/DiaDisponible")]
public class DiaDisponible : ScriptableObject
{
    public int numeroDia;
    public List<DefaultObject> alimentosPermitidos;
}
