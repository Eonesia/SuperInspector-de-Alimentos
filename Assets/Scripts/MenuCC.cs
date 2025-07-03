using UnityEngine;

public class MenuCC : MonoBehaviour
{
    public GameObject objetoMenuCuaderno;
    public GameObject objetoMenuCalculadora;

    public MenuInspeccion menuInspeccion; // NUEVO: Referencia al menú de inspección

    public bool Cuaderno = false;

    public void AlternarCuaderno()
    {
        // Bloquear si el menú de inspección no está activo
        if (menuInspeccion != null && !menuInspeccion.inspeccion)
            return;

        Cuaderno = !Cuaderno;
        objetoMenuCuaderno.SetActive(Cuaderno);
        objetoMenuCalculadora.SetActive(!Cuaderno);

        Time.timeScale = Cuaderno || !Cuaderno ? 0f : 1f;

        bool mostrarCursor = Cuaderno || objetoMenuCalculadora.activeSelf;
        Cursor.lockState = mostrarCursor ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = mostrarCursor;
    }
}