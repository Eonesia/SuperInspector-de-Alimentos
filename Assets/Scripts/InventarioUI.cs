using UnityEngine;

public class InventarioUI : MonoBehaviour
{
    public InventoryObject inventario;

    public void SeleccionarAlimento(int indice)
    {
        if (indice < 0 || indice >= inventario.Container.Count) return;

        ItemObject item = inventario.Container[indice].item;

        if (item is DefaultObject alimento)
        {
            // Crear un GameObject temporal con AlimentoInteractivo
            GameObject go = new GameObject("AlimentoEnMano");
            AlimentoInteractivo interactivo = go.AddComponent<AlimentoInteractivo>();
            interactivo.datosAlimento = alimento;

            FindObjectOfType<PlayerInteract>().TomarAlimento();
        }
        else
        {
            Debug.Log("Este objeto no es un alimento evaluable.");
        }
    }
}
