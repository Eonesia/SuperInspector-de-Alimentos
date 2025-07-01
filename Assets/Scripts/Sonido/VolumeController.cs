//using UnityEngine;
//using UnityEngine.Audio;
//using UnityEngine.UI;

//public class VolumeController : MonoBehaviour
//{
//    public AudioMixer audioMixer;
//    public Slider volumeSlider;

//    private void Start()
//    {
//        float savedVolume = PlayerPrefs.GetFloat("Master", 1f);
//        volumeSlider.value = savedVolume;
//        SetVolume(savedVolume);
//    }

//    public void SetVolume(float volume)
//    {
//        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
//        audioMixer.SetFloat("MasterVolume", dB);
//        PlayerPrefs.SetFloat("MasterVolume", volume);

//        Debug.Log(dB);
//    }
//}
