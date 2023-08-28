using System;
using UnityEngine;

public abstract class GameState : IDisposable
{
    protected ScoreBar scoreBar;
    protected IStateHandler<GameState> stateHandler;

    protected GameState(ScoreBar bar, IStateHandler<GameState> handler)
    {
        scoreBar = bar;
        stateHandler = handler;
    }

    protected void SwitchState<T>() where T : GameState
    {
        Debug.Log($"State has been switched to {typeof(T)}");
        stateHandler.SwitchState<T>();
    }

    public abstract void Dispose();
    public abstract void Update();
    public abstract void Activate();
    public abstract void Freeze();
}