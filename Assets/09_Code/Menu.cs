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
    private string levelToLoad;
    [SerializeField] private GameObject NoSaveGame = null;

    // Called when we click the "Yes" button in New Game
    public void NewGameYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    // Called when we click the "Yes" button in Load Game
    public void LoadGameYes()
    {
        if(PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
        }
        else
        {
            NoSaveGame.SetActive(true);
        }
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
    }
    public IEnumerator ConfirmationBox()
    {
        comfirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(3);
        comfirmationPrompt.SetActive(false);
    }





}
