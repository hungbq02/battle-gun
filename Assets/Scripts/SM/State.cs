using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected PlayerController _player;

    public State(PlayerController player)
    {
        _player = player;
    }   

    public virtual void Enter()
    {

    }
    public virtual void Exit()
    {

    }
    public abstract void Update();

}
