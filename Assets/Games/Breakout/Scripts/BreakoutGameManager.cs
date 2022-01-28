using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakoutGameManager : MonoBehaviour
{
    public BreakoutPlayerController playerBatPrefab;
    private BreakoutPlayerController playerBat;

    public void Init()
    {
        // if we have not defined a playerBat instance from the hierarchy, create one from the prefab
        if (!playerBat) playerBat = Instantiate(playerBatPrefab, transform, this);

        EntityInput input = playerBat.GetComponent<EntityInput>();
        input.Init(true);
        playerBat.Initialise(input);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
