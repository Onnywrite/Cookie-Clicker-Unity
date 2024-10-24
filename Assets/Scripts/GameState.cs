using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public abstract class GameState : IDisposable
{
    protected readonly ScoreBar scoreBar;
    protected readonly IStateHandler<GameState> stateHandler;
    protected readonly AudioSource audio;
    protected readonly AudioFactory sounds;

    protected GameState(ScoreBar bar, IStateHandler<GameState> handler, AudioSource asource, AudioFactory sounds)
    {
        scoreBar = bar;
        stateHandler = handler;
        audio = asource;
        this.sounds = sounds;
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

public class LoseState : GameState
{
    private Scene _scene;

    public LoseState(ScoreBar bar,
        IStateHandler<GameState> handler,
        AudioSource asource,
        AudioFactory sounds)
        : base(bar, handler, asource, sounds)
    {
        _scene = SceneManager.GetSceneByName("Lose");
    }

    public override void Activate()
    {
        SceneManager.LoadScene("Lose");
    }

    public override void Dispose()
    {
        
    }

    public override void Freeze()
    {
        
    }

    public override void Update()
    {
        
    }
}
public class WinState : GameState
{
    private Scene _scene;

    public WinState(ScoreBar bar,
        IStateHandler<GameState> handler,
        AudioSource asource,
        AudioFactory sounds)
        : base(bar, handler, asource, sounds)
    {
        _scene = SceneManager.GetSceneByName("Win");
    }

    public override void Activate()
    {
        SceneManager.LoadScene("Win");
    }

    public override void Dispose()
    {
        
    }

    public override void Freeze()
    {
        
    }

    public override void Update()
    {
        
    }
}