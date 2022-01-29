using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(SpriteRenderer))]
public class SpaceInvadersEnemy : MonoBehaviour, IShootableSpaceInvaders
{

    public Action<int> OnKilled;
    private SpaceInvadersEnemyGroupController controller;

    public EnemyType enemyType;

    public int walkStep = 0; // controls which sprite index is displayed
    public Sprite[] sprites;
    private SpriteRenderer renderer;
    
    public void Init(SpaceInvadersEnemyGroupController groupController)
    {
        controller = groupController;
        controller.OnEnemyMove += Move;
    }
    
    private void OnDisable()
    {
        if (enemyType != EnemyType.UFO) controller.OnEnemyMove -= Move;
        // This is used to make sure we stop the FreeMoveCoroutine - However according to
        // https://answers.unity.com/questions/34169/does-deactivating-a-gameobject-automatically-stop.html
        // SetActive(false) disables the object,therefore automatically stops the coroutine. But just to be safe
        // and make sure performance isn't affected stop all coroutines anyway.
        StopAllCoroutines();
    }

    public void OnEnable()
    {
        if (controller != null)
        {
            if (enemyType != EnemyType.UFO) controller.OnEnemyMove += Move;
        }
        
        renderer = renderer ? renderer : renderer = GetComponent<SpriteRenderer>();
        
        // reset animation
        walkStep = 0;
        renderer.sprite = sprites?.Length > 0 ? sprites[walkStep++] : renderer.sprite;
    }

    /// <summary>
    /// Called whenever it is hit by a bullet
    /// </summary>
    public void Hit()
    {
        gameObject.SetActive(false);
        switch (enemyType)
        {
            case EnemyType.Large:
                // 10
                OnKilled.Invoke(10);        
                break;
            case EnemyType.Medium:
                // 20
                OnKilled.Invoke(20);
                break;
            case EnemyType.Small:
                // 30
                OnKilled.Invoke(30);
                break;
            case EnemyType.UFO:
                // https://spaceinvaders.fandom.com/wiki/UFO
                // 50, 100, 150, 200 randomly : on 23rd shot, then 15th thereon 300 every time
                // we will keep the point gains random
                OnKilled.Invoke(50 * UnityEngine.Random.Range(1, 4));
                break;
        }
    }

    public void Move(Vector3 movement)
    {
        transform.position += movement;
        renderer.sprite = sprites.Length > 0 ? sprites[walkStep++] : renderer.sprite;
        walkStep = walkStep >= sprites.Length ? 0 : walkStep;
    }

    /// <summary>
    ///  Fire a bullet from this object
    /// </summary>
    public void Shoot()
    {
        // shoot
        SpaceInvadersBullet bullet = controller.bulletPool.SpawnBullet();
        bullet.transform.position = transform.position;
        bullet.Shoot(Vector2.down, this.gameObject, 8f);
    }

    /// <summary>
    /// The type of enemy determines the score.
    /// </summary>
    public enum EnemyType
    {
        Small,
        Medium,
        Large,
        UFO
    }

    /// <summary>
    /// Set the position and then start moving the UFO
    /// </summary>
    /// <returns></returns>
    IEnumerator FreeMoveCoroutine()
    {
        // choose a random direction
        float direction = Mathf.Sign(UnityEngine.Random.Range(-1.0f, 1.0f));
        // set the position to 10 units width opposite the direction of travel
        transform.position = new Vector3(10 * (-direction), 4, 0);
        while (isActiveAndEnabled)
        {
            yield return new WaitForEndOfFrame();
            if (Mathf.Abs(transform.position.x) > 10f)
            {
                gameObject.SetActive(false);
            }
            transform.position += new Vector3(direction * 2f, 0, 0) * Time.deltaTime;
            
        }
    }

}
