using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/AlimentoObjetivo")]
public class DefaultObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.AlimentoObjetivo;
    }
}

