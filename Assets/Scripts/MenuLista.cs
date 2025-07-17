using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

public class MenuLista : MonoBehaviour
{
    [System.Serializable]
    public class EntradaAlimento
    {
        public string nombre;
        public bool analizado = false;
    }

    public GameObject objetoMenuLista;
    public GameObject hud;
    public MenuInspeccion menuInspeccion;
    public MenuPausa menuPausa;

    public TextMeshProUGUI textoLista;
    public List<EntradaAlimento> alimentosDelDia;

    public bool lista = false;

    public void AlternarLista()
    {
        if (!lista &&
            ((menuInspeccion != null && menuInspeccion.inspeccion) ||
             (menuPausa != null && menuPausa.pausa)))
            return;

        lista = !lista;
        objetoMenuLista.SetActive(lista);
        hud.SetActive(!lista);
    }

    public void MarcarComoAnalizado(string nombre)
    {
        string nombreNormalizado = NormalizarTexto(nombre);

        foreach (var entrada in alimentosDelDia)
        {
            if (NormalizarTexto(entrada.nombre) == nombreNormalizado && !entrada.analizado)
            {
                entrada.analizado = true;
                Debug.Log("Marcando como analizado");
                break;
            }
        }

        ActualizarTexto();
    }

    private void ActualizarTexto()
    {
        string resultado = "";

        foreach (var entrada in alimentosDelDia)
        {
            if (entrada.analizado)
            {
                resultado += $"• <s><color=#888888>{entrada.nombre}</color></s>\n";
            }
            else
            {
                resultado += $"• {entrada.nombre}\n";
            }
        }

        textoLista.text = resultado;
        Debug.Log(textoLista.text);
    }

    private void Start()
    {
        ActualizarTexto();
    }

    private string NormalizarTexto(string texto)
    {
        string textoFormateado = texto.Trim().ToLower();
        string textoSinTildes = new string(textoFormateado
            .Normalize(NormalizationForm.FormD)
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            .ToArray());
        return textoSinTildes.Normalize(NormalizationForm.FormC);
    }
}


