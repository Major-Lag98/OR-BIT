using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{

    
    [SerializeField] GameObject _world;
    
    [SerializeField] float _gravityScale = 1;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float distance = Vector2.Distance(_world.transform.position, this.transform.position);

        Vector3 direction = _world.transform.position - transform.position;
        rb.AddForce(direction.normalized * _gravityScale);
    }
}
