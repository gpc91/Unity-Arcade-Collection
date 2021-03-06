using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class SpaceInvadersEnemyGroupController : MonoBehaviour
{

    /// <summary>
    /// GameManager of this group
    /// </summary>
    private SpaceInvadersManager manager;
    public int level = 0;

    public SpaceInvadersEnemy enemySmall;
    public SpaceInvadersEnemy enemyMedium;
    public SpaceInvadersEnemy enemyLarge;
    public SpaceInvadersEnemy enemyUFO;

    private SpaceInvadersEnemy spawnedUFO;
    public float UFOSpawnHeight = 5; 

    /// <summary>
    /// How often to try and randomly spawn a UFO in seconds
    /// </summary>
    public float UFOSpawnRateSeconds = 2f;

    /// <summary>
    /// The chance of spawning a UFO
    /// </summary>
    [Range(0.0f, 1.0f)]
    public float UFOSpawnChance = 0.1f;

    // The bullet pool should exist as a component of the group controller
    internal SpaceInvadersBulletPool bulletPool;

    /// <summary>
    /// Controls the movement of any registered monobehaviour
    /// </summary>
    public Action<Vector3> OnEnemyMove;

    private const int columns = 11;
    private const int rows = 5;
    
    /// <summary>
    /// Grid of enemies used to check which columns are alive and which are dead
    /// </summary>
    private SpaceInvadersEnemy[,] enemyGrid = new SpaceInvadersEnemy[columns, rows];

    /// <summary>
    /// Used to store which columns are living and which are dead. This is used to speed up the column checks
    /// </summary>
    public List<int> livingColumns = new List<int>();

    /// <summary>
    /// Returns the left most column, or -1 if there are no columns or the column list is null
    /// </summary>
    public int leftColumn => livingColumns?.Count > 0 ? livingColumns[0] : -1;
    /// <summary>
    /// Returns the right most column, or -1 if there are no columns or the column list is null
    /// </summary>
    public int rightColumn => livingColumns?.Count > 0 ? livingColumns[livingColumns.Count - 1] : -1;

    public int enemiesAlive;

    public float secondsBetweenMove = 1f;
    public float minimumSecondsMove = 0.05f;

    public float MovementLeftAmount = 0.1f;
    public float MovementRightAmount = 0.3f;
    public float MovementDropAmount = 0.75f;
    /// <summary>
    /// direction of enemy travel : -1 = left, 1 = right
    /// </summary>
    private int direction = 1;

    /// <summary>
    /// Checks to see if a column contains any active (alive) enemies.
    /// </summary>
    /// <param name="column">The index of the column to check</param>
    /// <returns>true if column contains living enemy else false</returns>
    public bool columnAlive(int column)
    {
        int alive = 0;
        for (int i = 0; i < 5; i++)
        {
            alive += enemyGrid[column, i].gameObject.activeInHierarchy ? 1 : 0;
        }
        return alive > 0 ? true : false;
    }

    /// <summary>
    /// Get an enemy from a grid column
    /// </summary>
    /// <param name="column">index of columm</param>
    /// <param name="bottom">if true gets the bottom invader</param>
    /// <returns>Returns an invader in the column, or null if column is dead</returns>
    /// <exception cref="Exception"></exception>
    public SpaceInvadersEnemy GetEnemy(int column, bool bottom = false)
    {
        // column list is null, or there are no more columns, finally, column is dead... return null
        if(livingColumns == null || livingColumns.Count <= 0 || !columnAlive(column)) return null;
        SpaceInvadersEnemy enemy;
        for (int i = 0; i < 5; i++)
        {
            enemy = enemyGrid[column, (bottom ? 4 - i : i)];
            if (enemy && enemy.isActiveAndEnabled) return enemy;
        }
        return null;
    }

    /// <summary>
    /// Initialise the enemy group
    /// </summary>
    /// <param name="manager">The manager that owns this group</param>
    public void Init(SpaceInvadersManager manager)
    {
        this.manager = manager;
        secondsBetweenMove = manager.difficulty;
        manager.NextLevelAction += NextRound;
    
        SpawnEnemies(ref enemyGrid);

        // instantiate the UFO and deactivate
        spawnedUFO = Instantiate(enemyUFO);
        spawnedUFO.OnKilled += EnemyKilled;
        spawnedUFO.gameObject.SetActive(false);
        spawnedUFO.Init(this, false);

        bulletPool = GetComponent<SpaceInvadersBulletPool>();
    
        // start the enemies movement.
        StartCoroutine("EnemyMovementCycle");
        StartCoroutine("UFOSpawnCoroutine");
    }

    /// <summary>
    /// Spawn an enemy, register it to the OnKilled event and Init it
    /// </summary>
    /// <param name="enemyPrefab">the prefab to spawn</param>
    /// <returns>returns the spawned entity</returns>
    private SpaceInvadersEnemy SpawnEnemy(SpaceInvadersEnemy enemyPrefab)
    {
        SpaceInvadersEnemy enemy;
        enemy = Instantiate(enemyPrefab, transform);
        enemy.OnKilled += EnemyKilled;
        enemy.Init(this);
        return enemy;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="register"></param>
    /// <param name="enemy">if passed it will use the prefab to instantiate and register a new enemy, otherwise it will reset</param>
    private void SpawnEnemies(ref SpaceInvadersEnemy[,] enemies)
    {
        for (int row = 0; row < 5; row++)
        {
            for (int column = 0; column < 11; column++)
            {
                SpaceInvadersEnemy typeToSpawn = null;
                switch (row)
                {
                    // one row of smalls
                    case 0:
                        typeToSpawn = enemySmall;
                        livingColumns?.Add(column);
                        break;
                    case 1:
                    case 2:
                        typeToSpawn = enemyMedium;
                        break;
                    case 3:
                    case 4:
                        typeToSpawn = enemyLarge;
                        break;
                }
            
                // Spawn a new enemy and assign it to the array if the array element does not already exist.
                // if it does exist, set enemy to that element.
                SpaceInvadersEnemy enemy;
                enemies[column, row] = enemy = enemies[column, row] == null && typeToSpawn != null
                    ? SpawnEnemy(typeToSpawn)
                    : enemies[column, row];
            
                // Position and activate the enemy
                enemy.transform.position =
                    new Vector3(-5f + (1f * column), (transform.position.y + (-1 * row)) - (0.25f * (level % 10)), 0);
                enemy.gameObject.SetActive(true);
                enemiesAlive++;
            }
        }
    }

    /// <summary>
    /// Spawn the UFO
    /// </summary>
    private void SpawnUFO()
    {
        // if we have no instantiated the UFO (we should have) do so now.
        if (!spawnedUFO)
        {
            spawnedUFO = Instantiate(spawnedUFO);
            spawnedUFO.OnKilled += EnemyKilled;
        }

        // if we are not already spawned...
        if (!spawnedUFO.isActiveAndEnabled)
        {
            spawnedUFO.gameObject.SetActive(true);
            spawnedUFO.StartCoroutine("FreeMoveCoroutine");
        }
    }

    /// <summary>
    /// Update the position of all enemies in this group
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyMovementCycle()
    {
        while (livingColumns?.Count > 0)
        {
            // wait for set time
            yield return new WaitForSeconds(secondsBetweenMove);
        
            // move
            switch (direction)
            {
                case 1:
                    if (GetEnemy(rightColumn)?.transform.position.x > manager.GameBounds.x)
                    {
                        direction = -1;
                        OnEnemyMove?.Invoke(new Vector3(0, -MovementDropAmount, 0));
                    }
                    else
                    {
                        // move enemy
                        OnEnemyMove?.Invoke(new Vector3(MovementRightAmount * direction, 0, 0));
                    }

                    break;
                case -1:
                    if (GetEnemy(leftColumn)?.transform.position.x < -manager.GameBounds.x)
                    {
                        direction = 1;
                        OnEnemyMove?.Invoke(new Vector3(0, -MovementDropAmount, 0));
                    }
                    else
                    {
                        // move enemy
                        OnEnemyMove?.Invoke(new Vector3(MovementLeftAmount * direction, 0, 0));
                    }
                    break;
            }
        
            // shoot
            SpaceInvadersEnemy enemy = GetEnemy(Random.Range(0, 11), true);
            enemy?.Shoot();
        }
    }

    IEnumerator UFOSpawnCoroutine()
    {
        while (livingColumns?.Count > 0)
        {
            yield return new WaitForSeconds(UFOSpawnRateSeconds);
            if (Random.value <= UFOSpawnChance) SpawnUFO();
        }
    }

    /// <summary>
    /// Reset the enemies
    /// </summary>
    void NextRound()
    {
        // stop enemies from being affected by the movement cycle
        StopCoroutine("EnemyMovementCycle");
        StopCoroutine("UFOSpawnCoroutine");
    
        //despawn UFO
        if (spawnedUFO && spawnedUFO.gameObject.activeInHierarchy) spawnedUFO.gameObject.SetActive(false);
    
        // clean up bullets
        bulletPool.DespawnAll();
    
        // respawn enemies
        SpawnEnemies(ref enemyGrid);

        level++;
        secondsBetweenMove = 1;
        // reset direction and start the movement cycle
        direction = 1;
        StartCoroutine("EnemyMovementCycle");
        StartCoroutine("UFOSpawnCoroutine");
    }

    /// <summary>
    /// Increases the score and performs calculations to update the group information
    /// </summary>
    /// <param name="score">The score to add to the player for the kill</param>
    void EnemyKilled(int score)
    {
        enemiesAlive--;
        manager.Score += score;
        // check each currently living column in the list to see if that column is dead and if so, remove it.
        for (int column = livingColumns.Count-1; column >= 0; column--)
        {
            // if the column reference in the current list index is dead, remove it from the list
            if (!columnAlive(livingColumns[column]))
            {
                livingColumns.RemoveAt(column);
            }
        }
    
        // if there are fewer than 15 enemies decrease time between moves more than if there are more
        if (enemiesAlive < 15)
        {
            secondsBetweenMove = enemiesAlive == 1 ? 0.05f : secondsBetweenMove - 0.025f;
        }
        else
        {
            secondsBetweenMove -= 0.005f;
        }
    
        secondsBetweenMove = Mathf.Clamp(secondsBetweenMove, minimumSecondsMove, 1);

        // if we have killed all enemy columns, move to the next round
        if (livingColumns?.Count <= 0) manager.NextLevel();
    }

}
