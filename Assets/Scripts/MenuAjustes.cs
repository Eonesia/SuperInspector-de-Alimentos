using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuAjustes : MonoBehaviour
{
    [Header("Audio y Sensibilidad")]
    public AudioMixer audioMixer;
    public Slider volumenSlider;
    public Slider sensibilidadSlider;
    public FirstPersonController jugador;

    [Header("Referencias UI")]
    public GameObject menuAjustesUI;
    public GameObject botonInicial; // puedes asignar el volumenSlider aquí en el Inspector

    private void Start()
    {
        float volumenGuardado = PlayerPrefs.GetFloat("VolumenAudio", 0.75f);
        volumenSlider.value = volumenGuardado;
        CambiarVolumen();

        float sensibilidad = PlayerPrefs.GetFloat("Sensibilidad", 1f);
        sensibilidadSlider.value = sensibilidad;
        sensibilidadSlider.onValueChanged.AddListener(CambiarSensibilidad);

        BuscarJugador();
        CambiarSensibilidad(sensibilidad);
    }

    private void BuscarJugador()
    {
        jugador = FindObjectOfType<FirstPersonController>();
    }

    public void CambiarVolumen()
    {
        float volumenDb = Mathf.Log10(Mathf.Clamp(volumenSlider.value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("MasterVolume", volumenDb);
        PlayerPrefs.SetFloat("VolumenAudio", volumenSlider.value);
    }

    public void CambiarSensibilidad(float valor)
    {
        if (jugador != null)
        {
            jugador.sensibilidadRaton = valor;
            jugador.sensibilidadMando = valor * 150f;
        }
        PlayerPrefs.SetFloat("Sensibilidad", valor);
    }

    public void AbrirMenuAjustes()
    {
        if (menuAjustesUI != null)
            menuAjustesUI.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (botonInicial != null)
        {
            StartCoroutine(SeleccionarConRetraso(botonInicial));
        }
        else if (volumenSlider != null)
        {
            StartCoroutine(SeleccionarConRetraso(volumenSlider.gameObject));
        }
    }

    public void CerrarMenuAjustes()
    {
        if (menuAjustesUI != null)
            menuAjustesUI.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        EventSystem.current.SetSelectedGameObject(null);
    }

    private System.Collections.IEnumerator SeleccionarConRetraso(GameObject objeto)
    {
        yield return null; // espera 1 frame tras abrir el menú
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(objeto);
    }
}
