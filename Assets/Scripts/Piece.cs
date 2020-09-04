using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{

    enum PieceState
    {
        Idle,
        Moving,
        Used,
        CrossedLine //maybe use this to discentigrate object like in Trickey Towers
    }

    PieceState _pieceState = PieceState.Idle;

    [SerializeField] GameObject _world;

    [SerializeField] float _gravityScale = 9.81f;

    Rigidbody2D rb;

    float waitTime; 
    [SerializeField] float WaitTimeMax = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
    private void Start()
    {
        waitTime = WaitTimeMax;
    }

    private void Update()
    {
        if (_pieceState == PieceState.Used) return; //do nothing if we have used

        

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
                Debug.Log(rb.velocity.magnitude);
                if (rb.velocity.magnitude <= 0.5f)
                {
                    waitTime -= Time.deltaTime;
                    if (waitTime <= 0)
                    {
                        _pieceState = PieceState.Used;
                        //transform.parent = _world.transform;
                        LevelStateMachine.Instance.ReadyNextPiece();
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
        if (_pieceState == PieceState.Moving || _pieceState == PieceState.Used)
        {
            //float distance = Vector2.Distance(_world.transform.position, this.transform.position);
            Vector3 direction = _world.transform.position - transform.position;
            rb.AddForce(direction.normalized * _gravityScale);
        }
    }
}
