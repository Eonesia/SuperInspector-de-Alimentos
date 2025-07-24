using UnityEngine;
using UnityEngine.EventSystems;

public class MenuCC : MonoBehaviour
{
    public GameObject objetoMenuCuaderno;
    public GameObject objetoMenuCalculadora;

    public MenuInspeccion menuInspeccion;

    public bool Cuaderno = false;

    // NUEVO: botón inicial del menú cuaderno para navegación con mando
    public GameObject botonInicialCuaderno;

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

        // NUEVO: Seleccionar botón inicial para navegación con mando
        if (Cuaderno && botonInicialCuaderno != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(botonInicialCuaderno);
        }
    }
}