using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakoutBallController : MonoBehaviour
{

    public float speed = 8;
    
    private Rigidbody2D r2d;

    void ShootBall(Vector2 direction)
    {
        r2d.AddForce(direction * (speed * 10));        
    }

    private void OnEnable()
    {
        if (!r2d) r2d = GetComponent<Rigidbody2D>();
        ShootBall(Vector2.up);
    }

    // Start is called before the first frame update
    void Start()
    {
        ShootBall(Vector2.up);
    }
}
