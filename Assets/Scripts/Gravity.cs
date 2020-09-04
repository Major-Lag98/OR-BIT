using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//NOW OBSOLETE



public class Gravity : MonoBehaviour
{

    
    [SerializeField] GameObject _world;
    
    [SerializeField] float _gravityScale = 9.81f;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() //fixedupdate should be used for physics
    {
        float distance = Vector2.Distance(_world.transform.position, this.transform.position);
        Vector3 direction = _world.transform.position - transform.position;
        rb.AddForce(direction.normalized * _gravityScale);
    }
}
