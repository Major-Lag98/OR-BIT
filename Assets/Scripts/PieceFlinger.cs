using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles flinging of pieces and force indicator
/// </summary>
public class PieceFlinger : MonoBehaviour
{

    // Our singleton instance
    public static PieceFlinger Instance;

    //The spawn position of the piece
    public Transform SpawnPosition;

    // The current piece to be flung. Set by the LevelStateMachine
    public Transform CurrentPieceBeingFlung;

    // The max distance we can drag
    float maxDistanceDrag = 1;

    [SerializeField]
    float maxFlingForce = 1;

    [SerializeField]
    [Tooltip("The distance from the spawn circle that a touch will start dragging")]
    float touchDistanceToStartFlinging = 0.5f;

    // If we're flinging
    public bool isFlinging = false;

    LineRenderer forceIndicator;


    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        forceIndicator = GameAssets.Instance.ForceIndicator;
        forceIndicator.SetPosition(1, SpawnPosition.position); //index 1 is the pointy end of our arrow bacause thats how i drew the sprite lol
    }

    // Update is called once per frame
    void Update()
    {
        // Don't do anything if we have no piece assigned
        if (!CurrentPieceBeingFlung)
            return;

        if(Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
        {
            forceIndicator.SetPosition(0, SpawnPosition.position);
            forceIndicator.gameObject.SetActive(true);
            var world = Camera.main.ScreenToWorldPoint(Input.touches[0].position); // Get the world
            

            // We are flinging if we clicked within the acceptable distance
            isFlinging = Vector3.Distance(new Vector3(SpawnPosition.position.x, SpawnPosition.position.y), new Vector3(world.x, world.y)) <= touchDistanceToStartFlinging;

        }else if (isFlinging && Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Moved)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.touches[0].position); // Get our world position from the touch
            pos = new Vector3(pos.x, pos.y, 0); // Recreate the vector to zero out the Z coordinate

            var spawnPos = new Vector3(SpawnPosition.position.x, SpawnPosition.position.y, 0); // Zero ou the z value of our spawn position by creating a new vector

            var distance = Vector3.Distance(new Vector3(pos.x, pos.y), new Vector3(spawnPos.x, spawnPos.y));  // Get the distance from the Spawn to the touch position

            /*
             * We do MaxDistanceDrag / distance so that it maxes out at 1 when we drag past the maxDrag distance.
             * ex: maxDragDistance = 1, distance (which is our distance from the SpawnPostion to our touch) = 0.5. 1/0.5 = 2 which gets clamped to 1. So t = 1 which means our piece is at our touch
             * ex2: maxDragDistance = 1, distance = 2. 1/2 = 0.5 = t. So we lerp the position at 0.5 between our spawn position and our touch position
             */
            var t = Mathf.Clamp(maxDistanceDrag / distance, 0, 1);
            CurrentPieceBeingFlung.position = Vector3.Lerp(spawnPos, pos, t); // Lerp between the two positions
            forceIndicator.SetPosition(0, CurrentPieceBeingFlung.position);

        }else if(isFlinging && Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Ended)
        {
            // Pattern match for easy Piece piece and Rigidbody2D rb references
            if(CurrentPieceBeingFlung.GetComponent<Piece>() is Piece piece && CurrentPieceBeingFlung.GetComponent<Rigidbody2D>() is Rigidbody2D rb)
            {
                piece.SetState(Piece.PieceState.Moving); // Set state to moving
                LevelStateMachine.Instance.state = LevelStateMachine.State.Playing; // Set level state machine to playing

                // Get magnitude
                var dir = CurrentPieceBeingFlung.position - SpawnPosition.position;

                // Add some force
                rb.AddForce(dir * -1 * maxFlingForce);

                // Null out our being flund object
                CurrentPieceBeingFlung = null;
            }

            isFlinging = false;
            forceIndicator.gameObject.SetActive(false);
        }
        

    }
}
