using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

/// <summary>
/// A barrier made up of individual BarrierBlocks
/// </summary>
public class SpaceInvadersBarrier : MonoBehaviour
{

    public SpaceInvadersManager manager;
    public SpaceInvadersBarrierBlock[] blocks;
    
    private void OnEnable()
    {
        if (manager) manager.NextLevelAction += ResetBlocks;
    }

    private void OnDisable()
    {
        if (manager) manager.NextLevelAction -= ResetBlocks;
    }

    /// <summary>
    /// Reset the Blocks that make up this barrier
    /// </summary>
    private void ResetBlocks()
    {
        foreach (SpaceInvadersBarrierBlock block in blocks)
        {
            if (!block.isActiveAndEnabled) block.gameObject.SetActive(true);
        }
        BroadcastMessage("Reset");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
