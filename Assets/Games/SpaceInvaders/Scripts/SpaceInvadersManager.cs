using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceInvadersManager : GameManager
{
    public SpaceInvadersShip playerShip;
    public SpaceInvadersEnemyGroupController groupControllerPrefab;
    private SpaceInvadersEnemyGroupController groupController;

    public SpaceInvadersBarrier baseBarrierPrefab; 
    
    public float difficulty = 1f;
    public int score = 0;

    public Vector2 GameBounds = new Vector2(7f, 5f);

    public Action NextLevelAction;
    
    public void Init()
    {
        // create ship
        playerShip = Instantiate(playerShip, transform, true);

        // subscribe player to the input manager
        EntityInput input = playerShip.GetComponent<EntityInput>();
        input.Init(true);
        playerShip.Initialise(this, input);

        // if there is no group controller in the scene, we should at least have the prefab attached to the manager
        // so, create the group controller as a child of the manager instance.
        if (groupController == null) groupController = Instantiate(groupControllerPrefab, transform, true);
        groupController.Init(this);

    }
    
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// End the game and invokes the action to end the game (not yet implemented)
    /// </summary>
    public void GameOver()
    {
        Debug.Log("Game Over!");
    }

    /// <summary>
    /// Advances the game to the next level and invokes the action to reset game pieces
    /// </summary>
    public void NextLevel()
    {
        Debug.Log("Next level...");
        NextLevelAction.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
