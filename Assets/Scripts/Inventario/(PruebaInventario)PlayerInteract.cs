using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public InventoryObject inventory;
    public Transform comidaCogida;
    public Transform Mano;
    
    



void Update()
    {
        

        if (Input.GetMouseButtonDown(0)) // Clic izquierdo
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var item = hit.collider.GetComponent<Item>();
                if (item)
                {
                    inventory.AddItem(item.item, 1);
                    Transform comidaCogida = item.transform;
                    Rigidbody rb = comidaCogida.GetComponent<Rigidbody>();
                    rb.isKinematic = true;
                    comidaCogida.position = Mano.position;
                    //comidaCogida.rotation = Mano.rotation;
                    comidaCogida.SetParent(Mano);
                }
            }
        }
    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    var item = other.GetComponent<Item>();
    //    if (item)
    //    {
    //        inventory.AddItem(item.item, 1);
    //        comidaCogida = item.transform;

    //        comidaCogida.position = Mano.position;
    //        comidaCogida.rotation = Mano.rotation;
    //        comidaCogida.SetParent(Mano);
    //    }
    //}

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
}


