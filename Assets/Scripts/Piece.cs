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
        Exploding,
        CrossedLine //maybe use this state to discentigrate object like in Trickey Towers
    }

    PieceState _pieceState = PieceState.Idle;

    [SerializeField] GameObject _planet;

    [SerializeField] float _gravityScale = 9.81f;

    float _acceptable_Resting_Velocity = 0.05f;

    [SerializeField, Tooltip("Second until piece deletes itself after win")]
    float explodedTimeout = 1f; 

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
        if (_pieceState == PieceState.Resting) return; //do nothing if this piece is resting
        
        if (_pieceState == PieceState.Exploding)
        {
            explodedTimeout -= Time.deltaTime;
            if (explodedTimeout <= 0)
            {
                Destroy(this.gameObject); //clear up some resorces after win
            }
        }



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
                if (rb.velocity.magnitude <= _acceptable_Resting_Velocity) //if the object is moving super slow we can pretty much say were resting
                {
                    waitTime -= Time.deltaTime;
                    if (waitTime <= 0) //we waited long enough for movement, we are resting
                    {
                        _pieceState = PieceState.Resting;
                        if (Physics2D.IsTouching(myCollider, GameAssets.Instance.limitInScene)) //check if we are touching the limit
                        {
                            LevelStateMachine.Instance.OnLose();
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
                    _acceptable_Resting_Velocity += Time.deltaTime * 0.5f;
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
