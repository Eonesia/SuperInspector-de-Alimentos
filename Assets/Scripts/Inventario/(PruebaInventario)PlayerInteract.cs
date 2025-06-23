using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public InventoryObject inventory;
    public Transform comidaCogida;
    public Transform Mano;

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();
        if (item)
        {
            inventory.AddItem(item.item, 1);
            comidaCogida = item.transform;

            comidaCogida.position = Mano.position;
            comidaCogida.rotation = Mano.rotation;
            comidaCogida.SetParent(Mano);
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
}


