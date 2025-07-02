using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using TMPro;

public class ControlDisplayManager : MonoBehaviour
{
    public TMP_Text[] botonesUI;
    private string controlActual = "";

    private void Update()
    {
        // Detectar entrada real de Gamepad
        if (Gamepad.current != null && SePresionoBotonDelGamepad())
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

    private bool SePresionoBotonDelGamepad()
    {
        foreach (var control in Gamepad.current.allControls)
        {
            if (control is ButtonControl boton && boton.wasPressedThisFrame)
                return true;
        }
        return false;
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