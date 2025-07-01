using UnityEngine;

public class MenuCC : MonoBehaviour
{
    public GameObject objetoMenuCuaderno;
    public GameObject objetoMenuCalculadora;

    public bool Cuaderno = false;

    public void AlternarCuaderno()
    {
        Cuaderno = !Cuaderno;
        objetoMenuCuaderno.SetActive(Cuaderno);
        objetoMenuCalculadora.SetActive(!Cuaderno);

        Time.timeScale = Cuaderno ? 0f : 1f;
        Cursor.lockState = Cuaderno ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = Cuaderno;

    }
}