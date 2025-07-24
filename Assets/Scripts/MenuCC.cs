using UnityEngine;
using UnityEngine.EventSystems;

public class MenuCC : MonoBehaviour
{
    public GameObject objetoMenuCuaderno;
    public GameObject objetoMenuCalculadora;

    public MenuInspeccion menuInspeccion;

    public bool Cuaderno = false;

    [Header("Botones para navegación con mando")]
    public GameObject botonInicialCuaderno;
    public GameObject botonInicialCalculadora;

    public void AlternarCuaderno()
    {
        // Bloquear si el menú de inspección no está activo
        if (menuInspeccion != null && !menuInspeccion.inspeccion)
            return;

        Cuaderno = !Cuaderno;
        objetoMenuCuaderno.SetActive(Cuaderno);
        objetoMenuCalculadora.SetActive(!Cuaderno);

        Time.timeScale = 0f; // Pausar correctamente

        bool mostrarCursor = Cuaderno || objetoMenuCalculadora.activeSelf;
        Cursor.lockState = mostrarCursor ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = mostrarCursor;

        // Seleccionar el botón inicial adecuado según el menú activo
        if (Cuaderno && botonInicialCuaderno != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(botonInicialCuaderno);
        }
        else if (!Cuaderno && botonInicialCalculadora != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(botonInicialCalculadora);
        }
    }
}