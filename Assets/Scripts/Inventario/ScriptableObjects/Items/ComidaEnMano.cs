using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New AlimentoEnMano Object", menuName = "Inventory System/Items/AlimentoEnMano")]

public class ComidaEnMano : ItemObject
{
    public float atqbonus;
    public float defbonus;

    public void Awake()
    {

        type = ItemType.AlimentoEnMano;
    }
}
