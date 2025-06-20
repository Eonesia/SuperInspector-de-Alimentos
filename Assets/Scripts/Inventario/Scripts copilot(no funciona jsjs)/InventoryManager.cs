using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public Transform handTransform; // Asigna aquí el GameObject vacío "Hand"
    public int maxItems = 3;
    private List<GameObject> inventory = new List<GameObject>();
    private int currentIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipItem(2);
    }

    public void AddItem(GameObject itemPrefab)
    {
        if (inventory.Count >= maxItems) return;

        GameObject itemInstance = Instantiate(itemPrefab, handTransform);
        itemInstance.transform.localPosition = Vector3.zero;
        itemInstance.transform.localRotation = Quaternion.identity;
        itemInstance.SetActive(false);
        inventory.Add(itemInstance);

        if (inventory.Count == 1)
            EquipItem(0);
    }

    void EquipItem(int index)
    {
        if (index < 0 || index >= inventory.Count) return;

        for (int i = 0; i < inventory.Count; i++)
            inventory[i].SetActive(i == index);

        currentIndex = index;
    }
}

