using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpaceInvadersShip : MonoBehaviour
{
    private SpaceInvadersManager manager;
    private InputBase inputController = null;

    public SpaceInvadersBullet bulletPrefab;

    private SpaceInvadersBullet bullet;
    
    private int lives = 3;
    public float speed = 2f;
    private bool canShoot = true;

    public Vector3 startingPosition = new Vector3(0, -6.0f, 0);

    /// <summary>
    /// Time in seconds between shots
    /// </summary>
    public float ShotCooldown = 1.0f;
    
    public void Initialise(SpaceInvadersManager manager, InputBase inputController)
    {
        this.manager = manager;
        this.inputController = inputController;
        transform.position = startingPosition;
        OnEnable();

        bullet = Instantiate(bulletPrefab, null, true);
        bullet.gameObject.SetActive(false);

    }

    /// <summary>
    /// Move the player left by the amount given in the speed variable
    /// </summary>
    public void MoveLeft()
    {
        if (transform.position.x > (-manager.GameBounds.x + (speed*Time.deltaTime)))
        {
            transform.position -= new Vector3(speed, 0, 0) * Time.deltaTime;    
        }
    }

    public void MoveRight()
    {
        if (transform.position.x < (manager.GameBounds.x - (speed*Time.deltaTime)))
        {
            transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;    
        }
    }
    
    public void Shoot()
    {
        if (!bullet.isActiveAndEnabled && canShoot)
        {
            bullet.gameObject.SetActive(true);
            bullet.transform.position = transform.position;
            bullet.Shoot(Vector2.up, this.gameObject);
            canShoot = false;
            StartCoroutine("ShotCooldownTimer");
        }
    }

    /// <summary>
    /// This reset the ability for the player to shoot after the seconds specified in the ShotCooldown property have elapsed
    /// </summary>
    /// <returns></returns>
    IEnumerator ShotCooldownTimer()
    {
        yield return new WaitForSecondsRealtime(ShotCooldown);
        canShoot = true;
    }
    
    
    public void OnEnable()
    {
        // when object is enabled subscribe to the input events
        if (inputController)
        {
            inputController.ActionAction += Shoot;
            inputController.ActionLeft += MoveLeft;
            inputController.ActionRight += MoveRight;
        }
    }
    
    public void OnDisable()
    {
        // unsubscribe from input events when object is disabled
        inputController.ActionAction -= Shoot;
        inputController.ActionLeft -= MoveLeft;
        inputController.ActionRight -= MoveRight;
    }
    
}
