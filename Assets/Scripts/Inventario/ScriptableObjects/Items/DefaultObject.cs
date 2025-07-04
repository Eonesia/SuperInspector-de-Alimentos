using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/AlimentoObjetivo")]
public class DefaultObject : ItemObject
{
    [Header("Evaluación del alimento")]
    [Range(1, 6)]
    public int calidadReal; // Valor real de calidad del alimento (oculto al jugador)

    private void Awake()
    {
        type = ItemType.AlimentoObjetivo;
    }
}

