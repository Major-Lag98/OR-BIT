using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Space()]
    public GameObject BackButton;
    [Space()]
    public GameObject LevelSelectPanel;
    public ScrollRect PanelScrollRect;
    [Space()]
    public GameObject LevelSelectBTN;
    [Space()]
    public GameObject CreditsBTN;
    public GameObject CreditsPanel;
    [Space()]
    public Button[] LevelButtons;

    // Start is called before the first frame update
    void Start()
    {
        UpdateLevels();
    }

    public void LoadLevel(int LevelIndex) //used by buttons
    {
        try { SceneManager.LoadScene(LevelIndex); } //try catch for testing without crashing: index could ont exist
        catch { }
        
    }


    public void GoToLevelSelect()
    {
        LevelSelectBTN.SetActive(false);
        CreditsBTN.SetActive(false);
        BackButton.SetActive(true);
        PanelScrollRect.verticalNormalizedPosition = 1; //reset scroll back to top: 1 = top, 0 = bottom.
        LevelSelectPanel.SetActive(true);
    }

    public void GoToMenu()
    {
        LevelSelectBTN.SetActive(true);
        CreditsBTN.SetActive(true);
        BackButton.SetActive(false);
        LevelSelectPanel.SetActive(false);
        CreditsPanel.SetActive(false);
    }

    public void GoToCredits()
    {
        LevelSelectBTN.SetActive(false);
        CreditsBTN.SetActive(false);
        CreditsPanel.SetActive(false);
        BackButton.SetActive(true);
        CreditsPanel.SetActive(true);
    }

    public void ClearSave()
    {
        PlayerPrefs.DeleteAll();
        GameManager.Instance.UpdateValues();
        UpdateLevels();
    }

    public void UpdateLevels()
    {
        foreach (Button button in LevelButtons)
            button.interactable = false;

        for (int i = 0; i <= GameManager.Instance.levelsCompleted; i++) //at start unlock all level buttons we completed
        {
            LevelButtons[i].interactable = true;
        }
    }
}
