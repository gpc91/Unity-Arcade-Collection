using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpaceInvadersBullet : MonoBehaviour
{
    // the type name of the spawner
    private GameObject spawner;

    public void Shoot(Vector2 direction, GameObject spawner, float velocity = 12f)
    {
        this.spawner = spawner;
        GetComponent<Rigidbody2D>().AddForce(direction * velocity, ForceMode2D.Impulse);
        gameObject.SetActive(true);
    }
    
    private void OnDisable()
    {
        this.spawner = null;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if we are colliding with the spawner do nothing
        if (other.gameObject == spawner) return;

        IShootableSpaceInvaders shootable = other.gameObject.GetComponent<IShootableSpaceInvaders>();
        // deactivate the bullets and enemy
        shootable?.Hit();
        gameObject.SetActive(false);
    }
}
