using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStateMachine : MonoBehaviour
{
    [SerializeField] Transform planetPostion;
    [SerializeField] Transform spawnPosition;

    Animator uiAnimator;
    //GameManager gameManager;

    public delegate void OnStateChanged();

    OnStateChanged OnWinState;
    OnStateChanged OnLoseState;

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
        //gameManager = FindObjectOfType<GameManager>();
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

        int index = SceneManager.GetActiveScene().buildIndex; //on win save completed levels
        GameManager.Instance.CheckLevelIndexComplete(index);
        

        //uiAnimator.SetTrigger("OnWin");

        StartCoroutine(Explode());

        OnWinState?.Invoke();
    }

    public void OnLose() //called by piece if over line
    {
        state = State.Lose;
        Debug.Log("we have lost");
        uiAnimator.SetTrigger("OnLoss");

        OnLoseState?.Invoke();

    }

    IEnumerator Explode()
    {
        //yield return new WaitForSeconds(2);
        var pieces = FindObjectsOfType<Piece>(); // Very alden-like but shouldn't be an issue since it's a single call at the end of the game
        foreach(var piece in pieces)
        {
            piece.SetState(Piece.PieceState.Exploding); // New state to keep the pieces from moving towards the planet
            var dir = (piece.transform.position - planetPostion.position).normalized; // Get direction 
            dir.Scale(new Vector3(750,750,0)); // Scale by some force (magic number)
            var RB = piece.GetComponent<Rigidbody2D>(); // get reference
            RB.AddForce(dir); // Add force
            RB.drag = 0; // get rid of drag
        }
        yield break;
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

    public void AddOnWinDelegate(OnStateChanged del)
        => OnWinState += del;

    public void AddOnLoseDelegate(OnStateChanged del)
        => OnLoseState += del;

    public void RemoveOnWinDelegate(OnStateChanged del)
        => OnWinState -= del;

    public void RemoveOnLoseDelegate(OnStateChanged del)
        => OnLoseState -= del;
}
