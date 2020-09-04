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
        Instantiate(Piece, spawnPosition.position, Quaternion.identity);
        state = State.Idle;
    }
}
