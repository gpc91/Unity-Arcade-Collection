using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A component that makes up a barrier
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class SpaceInvadersBarrierBlock : MonoBehaviour, IShootableSpaceInvaders
{
    public int maxHitPoints = 3;
    public int hitpoints = 3;
    
    public Sprite[] sprites;
    private SpriteRenderer renderer;

    private void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        hitpoints = maxHitPoints;
        renderer = renderer ? renderer : GetComponent<SpriteRenderer>();
        renderer.sprite = hitpoints > 0 && sprites.Length > 0 ? sprites[hitpoints-1] : renderer.sprite;
    }

    public void Hit()
    {
        hitpoints--;
        if (hitpoints <= 0) gameObject.SetActive(false);
        renderer.sprite = hitpoints > 0 && sprites.Length > 0 ? sprites[hitpoints-1] : renderer.sprite;
    }
    
}
