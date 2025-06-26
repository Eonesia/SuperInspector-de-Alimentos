using UnityEngine;

public class MenuPausa : MonoBehaviour
{
    public GameObject objetoMenuPausa;
    public bool pausa = false;

    public void AlternarPausa()
    {
        pausa = !pausa;
        objetoMenuPausa.SetActive(pausa);

        Time.timeScale = pausa ? 0f : 1f;
        Cursor.lockState = pausa ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = pausa;
    }
}
