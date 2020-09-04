using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowRotate : MonoBehaviour
{

    Rigidbody2D rb;

    [SerializeField] float rotationSpeed = 10;

    [SerializeField] bool counterClockwise = false;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        rotationSpeed = counterClockwise ? rotationSpeed : -rotationSpeed; //which way are we rotating?
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       

        rb.MoveRotation(rb.rotation + rotationSpeed * Time.fixedDeltaTime); //MoveRotation(360 * rotationSpeed * Time.fixedDeltaTime);
    }

    
}
