using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("Levels")]
    public string _newGameLevel;
    [SerializeField] private GameObject NoSaveGame = null;

    // Called when we click the "Yes" button in New Game
    public void Play()
    {
        SceneManager.LoadScene("ForestScene");

    }


    // Called when we click the "Quit" button.
    public void ExitButton()
    {
        Application.Quit();
    }


    // Audio Settimgs

    [Header("Volume Settings")]
    [SerializeField] private TMP_Text TextVolumeValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private GameObject comfirmationPrompt = null;
    [SerializeField] private float defaultVolume = 50;
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        TextVolumeValue.text = volume.ToString("0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuTYpe)
    {
        if (MenuTYpe == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            TextVolumeValue.text = defaultVolume.ToString("0");
            VolumeApply();
        }

       
        if (MenuTYpe == "Graphics")
        {
            SliderBrightness.value = defaultBrightness;
            TextBrightnessValue.text = defaultBrightness.ToString("0");

            ToggleFullscreen.isOn = false;
            Screen.fullScreen = false;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);

            DropdownResolution.value = resolutions.Length;
            GraphicsApply();
         
        }

        }
    public IEnumerator ConfirmationBox()
    {
        comfirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(3);
        comfirmationPrompt.SetActive(false);
    }


    // Graphic Settimgs

    [Header("Graphic Settings")]
    [SerializeField] private Slider SliderBrightness = null;
    [SerializeField] private TMP_Text TextBrightnessValue = null;
    [SerializeField] private float defaultBrightness = 50;
    [Header("Resolution Dropdown")]

    [Space(10)]
    [SerializeField] private Toggle ToggleFullscreen;

    private bool _isFullscreen;
    private float _brightnessLevel;
    public TMP_Dropdown DropdownResolution;
    private Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        DropdownResolution.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i <resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x "+resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        DropdownResolution.AddOptions(options);
        DropdownResolution.value = currentResolutionIndex;
        DropdownResolution.RefreshShownValue();
    } 

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        TextBrightnessValue.text = brightness.ToString("0");
    }

    public void SetFullscreen(bool isFullscreen)
    {
        _isFullscreen =isFullscreen;
    }
    
    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        PlayerPrefs.SetInt("masterFullscreen", (_isFullscreen ? 1 : 0));
        Screen.fullScreen = _isFullscreen;

        StartCoroutine(ConfirmationBox());
    }

    

}
