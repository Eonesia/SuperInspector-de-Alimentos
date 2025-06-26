using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject objetoMenuPausa;
    public GameObject hud;
    public bool pausa = false;

    public void AlternarPausa()
    {
        pausa = !pausa;
        objetoMenuPausa.SetActive(pausa);
        hud.SetActive(!pausa);

        Time.timeScale = pausa ? 0f : 1f;
        Cursor.lockState = pausa ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = pausa;
    }

    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MenuInicio"); 
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
