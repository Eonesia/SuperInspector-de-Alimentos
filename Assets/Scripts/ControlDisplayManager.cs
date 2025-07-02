using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ControlDisplayManager : MonoBehaviour
{
    public TMP_Text[] botonesUI; // Arrastra aquí los textos que muestran las letras de control
    private string controlActual = "";

    private void Update()
    {
        // Detecta si se ha usado un mando este frame
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {
            if (controlActual != "Gamepad")
                ActualizarControles("Gamepad");
        }
        // Detecta si se ha usado teclado o ratón este frame
        else if ((Keyboard.current != null && Keyboard.current.wasUpdatedThisFrame) ||
                 (Mouse.current != null && Mouse.current.wasUpdatedThisFrame))
        {
            if (controlActual != "Keyboard")
                ActualizarControles("Keyboard");
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