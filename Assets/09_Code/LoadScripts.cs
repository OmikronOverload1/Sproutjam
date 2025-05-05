using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadScripts : MonoBehaviour
{
    [Header("General Setting")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private Menu menuController;

    [Header("Volume Settings")]
    [SerializeField] private TMP_Text TextVolumeValue = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("Brightness Settings")]
    [SerializeField] private Slider SliderBrightness = null;
    [SerializeField] private TMP_Text TextBrightnessValue = null;

    [Header("Fullscreen Settings")]
    [SerializeField] private Toggle ToggleFullscreen;

    [Header("Resolution Settings")]
    [SerializeField] private TMP_Dropdown DropdownResolution;

    private void Awake()
    {
        if (canUse)
        {
            if (PlayerPrefs.HasKey("MasterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                TextVolumeValue.text = localVolume.ToString("0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else
            {
                menuController.ResetButton("Audio");
            }

            if (PlayerPrefs.HasKey("masterFullscreen"))
            {
                int localFullscreen = PlayerPrefs.GetInt("masterFullscreen");

                if(localFullscreen == 1)
                {
                    Screen.fullScreen = true;
                    ToggleFullscreen.isOn = true;
                }
                else 
                {
                    Screen.fullScreen = true;
                    ToggleFullscreen.isOn = true;
                }

            if (PlayerPrefs.HasKey("masterBrightness"))
            {
                    float localBrightness = PlayerPrefs.GetFloat("masterBrightness");

                    TextBrightnessValue.text = localBrightness.ToString("0");
                    SliderBrightness.value = localBrightness;
            }

            if (PlayerPrefs.HasKey("masterResolution"))
                {
                    int localResolution = PlayerPrefs.GetInt("masterResolution");
                    DropdownResolution.value = localResolution;
                }
            }
        }
    }
}
