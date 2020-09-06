using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Piece : MonoBehaviour
{

    public enum PieceState
    {
        Idle,
        Moving,
        Resting,
        CrossedLine //maybe use this state to discentigrate object like in Trickey Towers
    }

    PieceState _pieceState = PieceState.Idle;

    [SerializeField] GameObject _planet;

    [SerializeField] float _gravityScale = 9.81f;

    Collider2D myCollider;

    Rigidbody2D rb;

    float waitTime; 
    [SerializeField] float WaitTimeMax = 2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
    }
    private void Start()
    {
        waitTime = WaitTimeMax;
        transform.Rotate(Vector3.forward * Random.Range(0, 360)); //piece will start with some random rotation
    }

    private void Update()
    {
        if (_pieceState == PieceState.Resting) return; //do nothing if we have used

        

        switch (LevelStateMachine.Instance.state)
        {
            case LevelStateMachine.State.Idle: //touching the screen makes the object fall toward the planet
                //foreach (Touch touch in Input.touches)
                //{
                //    if (touch.phase == TouchPhase.Began)
                //    {
                //        _pieceState = PieceState.Moving;
                //        LevelStateMachine.Instance.state = LevelStateMachine.State.Playing;
                //    }
                //}

                break;

            case LevelStateMachine.State.Playing: //We wait for the object to come to a rest
                if (rb.velocity.magnitude <= 0.5f) //if the object is moving super slow we can pretty much say were resting
                {
                    waitTime -= Time.deltaTime;
                    if (waitTime <= 0) //we waited long enough for movement, we are resting
                    {
                        _pieceState = PieceState.Resting;
                        if (Physics2D.IsTouching(myCollider, GameAssets.Instance.limitInScene)) //check if we are touching the limit
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
                    waitTime = WaitTimeMax; //reset timer if we suddenly move while attempting to rest
                }
                    
                break;
        }
    }

    public void SetState(PieceState state)
    {
        _pieceState = state;
    }

    private void FixedUpdate() //fixedupdate should be used for physics
    {
        if (_pieceState == PieceState.Moving || _pieceState == PieceState.Resting) //always be attracted toward planet
        {
            Vector2 directionToPlanet = _planet.transform.position - transform.position;
            rb.AddForce(directionToPlanet.normalized * _gravityScale);
        }
    }

}
