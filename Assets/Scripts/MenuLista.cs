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
        public int valoracion = -1; // -1 si no ha sido evaluado
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

    public void MarcarComoAnalizado(string nombre, int valoracion)
    {
        string nombreNormalizado = NormalizarTexto(nombre);

        foreach (var entrada in alimentosDelDia)
        {
            if (NormalizarTexto(entrada.nombre) == nombreNormalizado && !entrada.analizado)
            {
                entrada.analizado = true;
                entrada.valoracion = valoracion;
                Debug.Log($"Marcando como analizado con valoración {valoracion}");
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
                string spriteEstrella = ObtenerEtiquetaSprite(entrada.valoracion);
                resultado += $"• <s><color=#888888>{entrada.nombre}</color></s> {spriteEstrella}\n";
            }
            else
            {
                resultado += $"• {entrada.nombre}\n";
            }
        }

        textoLista.text = resultado;
        Debug.Log(textoLista.text);
    }

    private string ObtenerEtiquetaSprite(int valoracion)
    {
        switch (valoracion)
        {
            case 6: return "<sprite name=\"estrella verde oscuro\">";
            case 5: return "<sprite name=\"estrella verde claro\">";
            case 4: return "<sprite name=\"estrella amarillo\">";
            case 3: return "<sprite name=\"estrella naranja\">";
            case 2: return "<sprite name=\"estrella roja\">";
            default: return "<sprite name=\"estrella negra\">";
        }
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

    private void Start()
    {
        ActualizarTexto();
    }
}



