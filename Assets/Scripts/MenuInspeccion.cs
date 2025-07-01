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

        Time.timeScale = inspeccion ? 0f : 1f;
        Cursor.lockState = inspeccion ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = inspeccion;

    }
}
