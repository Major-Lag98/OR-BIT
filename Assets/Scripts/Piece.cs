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
                //Debug.Log(rb.velocity.magnitude);
                if (rb.velocity.magnitude <= 0.5f)
                {
                    waitTime -= Time.deltaTime;
                    if (waitTime <= 0)
                    {
                        _pieceState = PieceState.Resting;
                        ValidatePiecePlacement();
                        LevelStateMachine.Instance.ReadyNextPiece();
                        //transform.parent = _world.transform;
                    }
                }
                else
                {
                    waitTime = WaitTimeMax; //reset timer if we suddenly move while resting
                }
                    
                break;
        }
    }

    private void ValidatePiecePlacement()
    {
        var bounds = GetComponent<BoxCollider2D>().bounds;

        if (IsOutsideBounds(transform.position, bounds))
        {
            LevelStateMachine.Instance.state = LevelStateMachine.State.Lose;
            Debug.Log("we have lost");
        }
    }

    private bool IsOutsideBounds(Vector3 position, Bounds bounds)
    {
        // Get the four corners of the collider
        var tl = new Vector2( position.x - bounds.extents.x,  position.y + bounds.extents.y);
        var tr = new Vector2(position.x + bounds.extents.x, position.y + bounds.extents.y);
        var bl = new Vector2(position.x - bounds.extents.x, position.y - bounds.extents.y);
        var br = new Vector2(position.x + bounds.extents.x, position.y - bounds.extents.y);

        var worldCenter = new Vector2(_world.transform.position.x, _world.transform.position.y);

        Vector2[] points = new Vector2[] { tl, tr, bl, br }; // Put into an array because I want to be lazy and use System.Linq 

        //TODO MAgic number --- WARNING
        var distance = 2;

        // Then return if any of them are past the limit
        return points.Any(p => Vector2.Distance(p, worldCenter) >= distance); // Linq.Any returns true if ANY of the array elements return true for the passed in function
    }

    private void FixedUpdate() //fixedupdate should be used for physics
    {
        if (_pieceState == PieceState.Moving || _pieceState == PieceState.Resting)
        {
            //float distance = Vector2.Distance(_world.transform.position, this.transform.position);
            Vector3 direction = _world.transform.position - transform.position;
            rb.AddForce(direction.normalized * _gravityScale);
        }
    }
}
