using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class MenuInicio : MonoBehaviour
{
    public GameObject botonPrimeroSeleccionado;  // Asignar desde inspector el botón inicial
    public AudioSource audioSource;
    public AudioClip sonidoBoton1;
    public AudioClip sonidoBoton2;

    private void OnEnable()
    {
        // Establecer el primer objeto seleccionado cuando el menú se activa
        if (botonPrimeroSeleccionado != null)
        {
            EventSystem.current.SetSelectedGameObject(null); // Limpia selección previa
            EventSystem.current.SetSelectedGameObject(botonPrimeroSeleccionado);
        }
    }

    public void Jugar()
    {
        SceneManager.LoadScene("TextosPrincipio");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Salir");
    }
    public void AudioBoton1()
    {
        audioSource.PlayOneShot(sonidoBoton1);
    }
    public void AudioBoton2()
    {
        audioSource.PlayOneShot(sonidoBoton2);
    }
}
