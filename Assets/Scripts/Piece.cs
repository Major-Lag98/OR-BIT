using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Piece : MonoBehaviour
{

    enum PieceState
    {
        Idle,
        Moving,
        Resting,
        CrossedLine //maybe use this to discentigrate object like in Trickey Towers
    }

    PieceState _pieceState = PieceState.Idle;

    [SerializeField] GameObject _world;

    EdgeCollider2D _limit; //try to make this a reference!!

    [SerializeField] float _gravityScale = 9.81f;

    BoxCollider2D myCollider;

    Rigidbody2D rb;

    float waitTime; 
    [SerializeField] float WaitTimeMax = 2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        _limit = GameObject.Find("Limit").GetComponent<EdgeCollider2D>(); //I have to use this mess because referencing a prefab doesnt work for some reason... maybe make a singleton that holds objects in the scene to reference?
        
    }
    private void Start()
    {
        waitTime = WaitTimeMax;
    }

    private void Update()
    {
        if (_pieceState == PieceState.Resting) return; //do nothing if we have used

        

        switch (LevelStateMachine.Instance.state)
        {
            case LevelStateMachine.State.Idle: //touching the screen makes the object fall toward the planet
                foreach (Touch touch in Input.touches) 
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        _pieceState = PieceState.Moving;
                        LevelStateMachine.Instance.state = LevelStateMachine.State.Playing;
                    }
                }

                break;

            case LevelStateMachine.State.Playing: //We wait for the object to come to a rest
                if (rb.velocity.magnitude <= 0.5f)
                {
                    waitTime -= Time.deltaTime;
                    if (waitTime <= 0)
                    {
                        _pieceState = PieceState.Resting;
                        if (Physics2D.IsTouching(myCollider, _limit)) //check if we are touching the limit
                        {
                            Debug.Log("we have lost");
                            LevelStateMachine.Instance.state = LevelStateMachine.State.Lose;
                        }
                        else
                        {
                            LevelStateMachine.Instance.ReadyNextPiece();
                        }
                    }
                }
                else
                {
                    waitTime = WaitTimeMax; //reset timer if we suddenly move while resting
                }
                    
                break;
        }
    }

    private void FixedUpdate() //fixedupdate should be used for physics
    {
        if (_pieceState == PieceState.Moving || _pieceState == PieceState.Resting) //always be attracted toward planet
        {
            Vector2 directionToPlanet = _world.transform.position - transform.position;
            rb.AddForce(directionToPlanet.normalized * _gravityScale);
        }
    }
}
