using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour //use to save and load game data
{

    public static GameManager Instance;

    public int levelsCompleted { get; private set; }

    // OnEnable called before start
    void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            levelsCompleted = PlayerPrefs.GetInt("LevelsCompleted", 0);
        }
        else Destroy(this);
    }

    public void SetLevelsCompleted(int number)
    {
        PlayerPrefs.SetInt("LevelsCompleted", number);
        levelsCompleted = number;
    }

    public int GetLevelsCompleted()
    {
        return PlayerPrefs.GetInt("LevelsCompleted");
    }

    public void CheckLevelIndexComplete(int currIndex)
    {
        if (currIndex > levelsCompleted)
        {
            SetLevelsCompleted(currIndex);
        }
    }

    public void UpdateValues()
    {
        levelsCompleted = PlayerPrefs.GetInt("LevelsCompleted", 0);
    }
}
