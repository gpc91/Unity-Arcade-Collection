using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SpaceInvadersBarrierBlock : MonoBehaviour, IShootableSpaceInvaders
{
    public int maxHitPoints = 3;
    public int hitpoints = 3;

    private void Start()
    {
        hitpoints = maxHitPoints;
    }

    public void Hit()
    {
        hitpoints--;
        if (hitpoints <= 0) gameObject.SetActive(false);
        
        // scales the alpha based upon how many hitpoints remain
        // replace this with sprite swaps to indicate damage instead of alpha value.
        GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 1.0f / (maxHitPoints));
    }
    
}
