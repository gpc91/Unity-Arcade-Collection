using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputBase : MonoBehaviour
{
    public Action ActionUp;
    public Action ActionDown;
    public Action ActionLeft;
    public Action ActionRight;
    public Action ActionAction;
    public Action ActionPause;
}
