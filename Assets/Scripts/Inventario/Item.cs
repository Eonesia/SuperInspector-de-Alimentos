using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item : MonoBehaviour
{
    public ItemObject item;
    public Vector3 rotacionEnMano; // editable desde el Inspector
    public Vector3 posicionEnMano;
    public Vector3 posicionInspeccionPersonalizada = Vector3.zero;
    public Vector3 rotacionInspeccionPersonalizada = Vector3.zero;
}

