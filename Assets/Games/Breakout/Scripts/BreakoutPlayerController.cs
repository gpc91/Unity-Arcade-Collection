using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakoutPlayerController : MonoBehaviour
{
    private InputBase inputController;
    public float speed = 4f;

    public BreakoutBallController ballPrefab;
    
    public void Initialise(InputBase input)
    {
        inputController = input;
        transform.position = new Vector3(0, -4, 0);
        
        // call this as OnEnable is invoked before Initialise
        OnEnable();
    }

    public void OnEnable()
    {
        if (inputController) // if we have an input controller and this is called, register our inputs
        {
            inputController.ActionRight += MoveRight;
            inputController.ActionLeft += MoveLeft;
            inputController.ActionAction += ShootBall;
        }
    }

    public void OnDisable()
    {
        inputController.ActionRight -= MoveRight;
        inputController.ActionLeft -= MoveLeft;
        inputController.ActionAction -= ShootBall;
    }

    public void MoveLeft()
    {
        if (transform.position.x > (-7 + (speed * Time.deltaTime)))
        {
            transform.position -= new Vector3(speed, 0, 0) * Time.deltaTime;
        }
        Debug.Log("Move left");
    }

    public void MoveRight()
    {
        if (transform.position.x < (7 - (speed * Time.deltaTime)))
        {
            transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
        }
        Debug.Log("Move right");
    }

    public void ShootBall()
    {
        // Here, when we are able to, we shoot the ball.
    }
    
}
