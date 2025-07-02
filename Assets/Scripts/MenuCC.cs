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

        Time.timeScale = Cuaderno || !Cuaderno ? 0f : 1f;

        // Mostrar el cursor si cualquiera de los dos menús está activo
        bool mostrarCursor = Cuaderno || objetoMenuCalculadora.activeSelf;

        Cursor.lockState = mostrarCursor ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = mostrarCursor;

    }
}