using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ControlDisplayManager : MonoBehaviour
{
    public TMP_Text[] botonesUI;
    private string controlActual = "";

    private void Update()
    {
        // Detectar entrada real de Gamepad
        if (Gamepad.current != null && Gamepad.current.allControls.Exists(c => c is ButtonControl btn && btn.wasPressedThisFrame))
        {
            if (controlActual != "Gamepad")
                ActualizarControles("Gamepad");
        }
        // Detectar entrada real de teclado
        else if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
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