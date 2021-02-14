using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButtons : MonoBehaviour
{

    [SerializeField, Header("Pause Menu References")]
    GameObject pausePanel;
    [SerializeField] GameObject pauseButton;

    public void NextLevel()
    {
        try { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); } // Load the next indexed level after this one
        catch { }
    }
    public void BackToMainMenu()
    {
        Time.timeScale = 1; // in case if we go here from being paused;
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    bool _paused = false; //start game not paused
    public void PauseSwitch() 
    {
        _paused = !_paused;
        Time.timeScale = _paused ? 0 : 1;
        pausePanel.SetActive(_paused);
        pauseButton.SetActive(!_paused);
    }
}
