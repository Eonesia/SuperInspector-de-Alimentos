using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items/Comida")]
public class FoodObject : ItemObject
{
    public int RestorHealthValue;
  public void Awake()
    {
        type = ItemType.Comida;
    }
}
