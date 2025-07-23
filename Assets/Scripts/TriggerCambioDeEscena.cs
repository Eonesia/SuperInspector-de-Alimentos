using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerCambioDeEscena : MonoBehaviour
{
    public string nombreEscenaDestino = "MenuInicio";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nombreEscenaDestino);
            Cursor.lockState = CursorLockMode.None;
Cursor.visible = true;
Time.timeScale = 1f;
        }
    }
}
