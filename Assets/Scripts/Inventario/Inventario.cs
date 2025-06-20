using UnityEngine;

public class Inventario :MonoBehaviour
{
    public Comida[] Comida = new Comida [4];

}

public enum Comida
{ 

 Vacío,
 Comida1,
 Comida2,
 Comida3,
 Comida4,

}