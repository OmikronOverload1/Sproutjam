using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
}
