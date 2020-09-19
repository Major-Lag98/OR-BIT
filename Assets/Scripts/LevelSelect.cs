using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{

    [SerializeField] Animator mainMenuAnimations;

    public void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    public void LevelSelectFocus()
    {
        mainMenuAnimations.SetTrigger("LevelSelectFocus"); //needs implementation
    }

    public void MainMenuFocus()
    {
        mainMenuAnimations.SetTrigger("MainMenuFocus"); //needs implementation
    }



}
