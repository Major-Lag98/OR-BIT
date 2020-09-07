using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStateMachine : MonoBehaviour
{

    [SerializeField] GameObject Piece = null;

    [SerializeField] Transform spawnPosition;

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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ReadyNextPiece();
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
    }

    void UpdateUIElements()
    {
        int amountRemaining = LevelData.Instance.PiecesQueue.Count;
        GameAssets.Instance.UIAmountLeft.SetText($"{amountRemaining} Left!");
    }
}
