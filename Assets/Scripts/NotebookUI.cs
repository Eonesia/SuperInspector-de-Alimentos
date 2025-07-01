using UnityEngine;
using UnityEngine.UI;

public class NotebookUI : MonoBehaviour
{
    public GameObject[] contentPages; // Cada índice es una "página" del cuaderno

    public void ShowPage(int index)
    {
        // Oculta todas las páginas
        for (int i = 0; i < contentPages.Length; i++)
        {
            contentPages[i].SetActive(i == index);
        }
    }
}
