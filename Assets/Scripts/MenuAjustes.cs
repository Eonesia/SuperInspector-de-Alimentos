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
        CambiarVolumen();
        

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

    public void CambiarVolumen()
    {
        float volumenDb = Mathf.Log10(Mathf.Clamp(volumenSlider.value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("MasterVolume", volumenDb);

        PlayerPrefs.SetFloat("VolumenAudio", volumenSlider.value);
        Debug.Log(volumenDb);
    }

    public void CambiarSensibilidad(float valor)
    {
        if (jugador != null)
        {
            jugador.sensibilidadRaton = valor;

            // Opcional: también puedes ajustar sensibilidadMando si quieres
            jugador.sensibilidadMando = valor * 150f;
        }

        // Guardar en PlayerPrefs
        PlayerPrefs.SetFloat("Sensibilidad", valor);
    }

}
