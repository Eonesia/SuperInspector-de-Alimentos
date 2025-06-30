using UnityEngine;

public class MenuInspeccion : MonoBehaviour
{
    public GameObject objetoMenuInspeccion;
    public GameObject hud;

    public bool inspeccion = false;

    public void AlternarInspeccion()
    {
        inspeccion = !inspeccion;
        objetoMenuInspeccion.SetActive(inspeccion);
        hud.SetActive(!inspeccion);

    }
}
