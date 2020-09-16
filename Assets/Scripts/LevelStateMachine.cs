using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStateMachine : MonoBehaviour
{

    [SerializeField] Transform spawnPosition;

    Animator uiAnimator;

    public enum State 
    {
        Intro,      //Let things smoothly settle in after scene load
        Idle,       //Is when you are waiting for input from the player to launch the object
        Playing,    //Is when you have launched an object and waiting for it to settle
        Pause,
        Lose,
        win
    }

    public State state;

    public static LevelStateMachine Instance; //make singleton

    private void Awake() //set singleton and grab the UI Animator
    {
        if (Instance == null)
        {
            Instance = this;
        }

        
    }

    private void Start()
    {
        ReadyNextPiece();
        uiAnimator = GameAssets.Instance.UI.GetComponent<Animator>();
    }


    public void ReadyNextPiece()
    {

        if (LevelData.Instance.PiecesQueue.Count <= 0) //if dont have any pieces left to ready we have won
        {
            OnWin();
            return;
        }
        PieceFlinger.Instance.CurrentPieceBeingFlung = Instantiate(LevelData.Instance.PiecesQueue.Dequeue(), spawnPosition.position, Quaternion.identity).transform;
        state = State.Idle;
        UpdateUIElements();
    }

    void OnWin()
    {
        state = State.win;
        Debug.Log("We win yay");

        var index = SceneManager.GetActiveScene().buildIndex;
        var last = PlayerPrefs.GetInt("levels");

        if (index > last)
        {
            PlayerPrefs.SetInt("levels", 1);
            PlayerPrefs.Save();
        }

        uiAnimator.SetTrigger("OnWin");
    }

    public void OnLose() //called by piece
    {
        state = State.Lose;
        Debug.Log("we have lost");
        uiAnimator.SetTrigger("OnLoss");
    }

    void UpdateUIElements()
    {
        int amountRemaining = LevelData.Instance.PiecesQueue.Count;
        GameAssets.Instance.UIAmountLeft.SetText($"{amountRemaining} Left!");
        UpdateNextPiecePreview();
    }

    void UpdateNextPiecePreview()
    {
        // PiecesQueue.Peek() will throw an exception if its empty, so we make sure its not here
        if (LevelData.Instance.PiecesQueue.Count > 0)
        {
            // Grab the sprite from the next (prefab) piece
            var nextPieceSprite = LevelData.Instance.PiecesQueue.Peek().GetComponent<SpriteRenderer>().sprite;

            // The width ratio.
            float widthRatio = (float)nextPieceSprite.texture.width / (float)nextPieceSprite.texture.height;

            // These two calculations/clamps make sure that the scales balance and never go past 1.
            float scaleY = Mathf.Clamp(1 / widthRatio, 0, 1); // Make sure if our width ratio is over 1, our Y scale will be brought down
            float scaleX = Mathf.Clamp(widthRatio, 0, 1); // Make sure to clamp between 0 and 1

            // Assign the sprite
            GameAssets.Instance.NextPieceImage.sprite = nextPieceSprite;

            // Set the scale
            GameAssets.Instance.NextPieceImage.transform.localScale = new Vector3(scaleX, scaleY, 1);
        }
        else //deactivate gameobject to not have a white box and get rid of the next text
        {
            GameAssets.Instance.NextPieceImage.sprite = null;
            GameAssets.Instance.NextPieceImage.transform.localScale = new Vector3(1, 1, 1);
            GameAssets.Instance.NextPieceImage.gameObject.SetActive(false); 
            GameAssets.Instance.UINextText.SetText("");
        }
    }
}
