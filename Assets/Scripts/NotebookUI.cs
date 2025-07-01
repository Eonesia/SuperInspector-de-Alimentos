using UnityEngine;
using UnityEngine.UI;

public class NotebookUI : MonoBehaviour
{
    public GameObject[] contentPages; // Cada �ndice es una "p�gina" del cuaderno

    public void ShowPage(int index)
    {
        // Oculta todas las p�ginas
        for (int i = 0; i < contentPages.Length; i++)
        {
            contentPages[i].SetActive(i == index);
        }
    }
}
