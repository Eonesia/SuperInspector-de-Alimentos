using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("TextosPrincipio");
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Salir");
    }
}
