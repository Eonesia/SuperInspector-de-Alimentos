using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class CalculatorController : MonoBehaviour
{
    public TMP_Text displayText;

    private string currentInput = "0";
    private string previousInput = "";
    private string operation = "";
    private bool isNewInput = false;

    public void OnNumberPressed(string number)
    {
        if (isNewInput || currentInput == "0")
            currentInput = number;
        else
            currentInput += number;

        isNewInput = false;
        UpdateDisplay();
    }

    public void OnDecimalPressed()
    {
        if (!currentInput.Contains("."))
            currentInput += ".";
        UpdateDisplay();
    }

    public void OnOperationPressed(string op)
    {
        if (!string.IsNullOrEmpty(operation))
            Calculate();

        previousInput = currentInput;
        operation = op;
        isNewInput = true;
    }

    public void OnEqualsPressed()
    {
        Calculate();
        operation = "";
    }

    public void OnClearEntry()
    {
        currentInput = "0";
        UpdateDisplay();
    }

    public void OnClearAll()
    {
        currentInput = "0";
        previousInput = "";
        operation = "";
        UpdateDisplay();
    }

    public void OnBackspace()
    {
        if (currentInput.Length > 1)
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
        else
            currentInput = "0";

        UpdateDisplay();
    }

    public void OnToggleSign()
    {
        if (currentInput.StartsWith("-"))
            currentInput = currentInput.Substring(1);
        else if (currentInput != "0")
            currentInput = "-" + currentInput;

        UpdateDisplay();
    }

    private void Calculate()
    {
        try
        {
            double a = Convert.ToDouble(previousInput);
            double b = Convert.ToDouble(currentInput);
            double result = 0;

            switch (operation)
            {
                case "+": result = a + b; break;
                case "-": result = a - b; break;
                case "*": result = a * b; break;
                case "/": result = b != 0 ? a / b : 0; break;
            }

            currentInput = result.ToString();
            isNewInput = true;
            UpdateDisplay();
        }
        catch
        {
            currentInput = "Error";
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        displayText.text = currentInput;
    }
}