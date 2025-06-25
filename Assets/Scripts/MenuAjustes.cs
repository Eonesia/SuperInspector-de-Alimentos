using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuAjustes : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumenSlider;
    public Slider sensibilidadSlider;
    public FirstPersonController jugador;

    void Start()
    {
        float volumenGuardado = PlayerPrefs.GetFloat("VolumenAudio", 0.75f);
        volumenSlider.value = volumenGuardado;
        CambiarVolumen(volumenGuardado);

        float sensibilidad = PlayerPrefs.GetFloat("Sensibilidad", 1f);
        sensibilidadSlider.value = sensibilidad;
        sensibilidadSlider.onValueChanged.AddListener(CambiarSensibilidad);

        // Intentamos encontrar al jugador si está en esta escena
        BuscarJugador();
        CambiarSensibilidad(sensibilidad);
    }

    void BuscarJugador()
    {
        jugador = FindObjectOfType<FirstPersonController>();
    }

    public void CambiarVolumen(float valor)
    {
        float volumenDb = Mathf.Log10(Mathf.Clamp(valor, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("MasterVolume", volumenDb);

        PlayerPrefs.SetFloat("VolumenAudio", valor);
    }

    public void CambiarSensibilidad(float valor)
    {
        PlayerPrefs.SetFloat("Sensibilidad", valor);

        if (jugador == null)
            BuscarJugador();

        if (jugador != null)
        {
            jugador.sensibilidadRaton = valor;
            jugador.sensibilidadMando = valor * 150f;
        }
    }

}
