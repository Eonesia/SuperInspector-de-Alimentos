using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("EscenaPrueba");
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Salir");
    }
}
