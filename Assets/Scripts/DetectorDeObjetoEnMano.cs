using UnityEngine;

public class DetectarObjetoActivo : MonoBehaviour
{
    public PlayerInteract player;

    private ItemObject ultimoObjeto;

    void Update()
    {
        if (player == null) return;

        ItemObject actual = player.TomarAlimento(); // o GetAlimentoSeleccionado() si no lo has renombrado

        if (actual != null && actual != ultimoObjeto)
        {
            Debug.Log("Nuevo objeto activo en la mano: " + actual.name);
            ultimoObjeto = actual;

            // Aqu� puedes a�adir l�gica adicional, como activar UI, reproducir sonido, etc.
        }
    }
}

