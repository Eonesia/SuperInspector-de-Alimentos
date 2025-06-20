using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public GameObject itemPrefab; // Asigna aqu� el prefab del objeto que se mostrar� en la mano

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            InventoryManager inv = other.GetComponent<InventoryManager>();
            if (inv != null)
            {
                inv.AddItem(itemPrefab);
                Destroy(gameObject);
            }
        }
    }
}
