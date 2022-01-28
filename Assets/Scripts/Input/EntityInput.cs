using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Processes a users input and invokes the actions that are subscribed to that users input.
/// </summary>
public class EntityInput : InputBase
{
    private string suffix = "";
    
    public void Init(bool P1)
    {
        suffix = P1 ? "P1" : "P2";
    }
    public void Update()
    {
        if (Input.GetButton($"Pause{suffix}")) ActionPause?.Invoke();
        if (Input.GetButton($"Action{suffix}")) ActionAction?.Invoke();
        if (Input.GetButton($"Up{suffix}"))  ActionUp?.Invoke();
        if (Input.GetButton($"Down{suffix}")) ActionDown?.Invoke();
        if (Input.GetButton($"Left{suffix}")) ActionLeft?.Invoke();
        if (Input.GetButton($"Right{suffix}")) ActionRight?.Invoke();
    }
    //TODO: Account for touch input through functions
    public void Pause() => ActionPause?.Invoke();
    public void Action() => ActionAction?.Invoke();
    public void Up() => ActionUp?.Invoke();
    public void Down() => ActionDown?.Invoke();
    public void Left() => ActionLeft?.Invoke();
    public void Right() => ActionRight?.Invoke();
}
