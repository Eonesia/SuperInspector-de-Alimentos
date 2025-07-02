using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ControlDisplayManager : MonoBehaviour
{
     public TMP_Text[] botonesUI;
    private string controlActual = "";

    private float tiempoUltimaEntrada = 0f;
    private float tiempoEspera = 0.2f;

    private void Update()
    {
        string nuevoControl = controlActual;

        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
            nuevoControl = "Gamepad";
        else if ((Keyboard.current != null && Keyboard.current.wasUpdatedThisFrame) ||
                 (Mouse.current != null && Mouse.current.wasUpdatedThisFrame))
            nuevoControl = "Keyboard";

        // Solo actualiza si ha pasado suficiente tiempo y el control ha cambiado
        if (nuevoControl != controlActual && Time.time - tiempoUltimaEntrada > tiempoEspera)
        {
            ActualizarControles(nuevoControl);
            tiempoUltimaEntrada = Time.time;
        }
    }

    

    private void ActualizarControles(string nuevoControl)
    {
        controlActual = nuevoControl;

        foreach (TMP_Text boton in botonesUI)
        {
            switch (boton.name)
            {
                case "ControlPausa":
                    boton.text = nuevoControl == "Gamepad" ? "opt" : "Esc";
                    break;
                case "ControlInspeccion":
                    boton.text = nuevoControl == "Gamepad" ? "Y" : "E";
                    break;
                case "ControlLista":
                    boton.text = nuevoControl == "Gamepad" ? "R1" : "Q";
                    break;
                case "ControlCuaderno":
                    boton.text = nuevoControl == "Gamepad" ? "L2" : "F";
                    break;
                case "ControlCalculadora":
                    boton.text = nuevoControl == "Gamepad" ? "L2" : "F";
                    break;
                // Añade más casos si tienes más botones
            }
        }
    }
}