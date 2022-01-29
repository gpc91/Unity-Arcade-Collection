using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface that allows game objects to be considered "shootable" by either players or enemies
/// </summary>
public interface IShootableSpaceInvaders
{
    /// <summary>
    /// Process a bullet hit
    /// </summary>
    public void Hit();
}
