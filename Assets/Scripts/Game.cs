using System.Linq;
using UnityEngine;

/// <summary>
/// Main game class, which takes control over all game states
/// represented in classes <see cref="StandartState"/> and <see cref="CrazyState"/>
/// and contains single intance of class <see cref="ScoreBar"/>
/// </summary>
// TODO: implement state pattern for stages like
// anarchy (a lotta small coockies),
// boosted (a lot more scores by click),
// boss (couple clicks to kill one cookie
// etc.
// Status: Almost done
public class Game : MonoBehaviour, IStateHandler<GameState>
{
    [SerializeField]
    private ObjectFactory _factory;

    [SerializeField]
    private AudioSource _audio;
    [SerializeField]
    private AudioFactory _sounds;

    [SerializeField]
    private ScoreBar _scoreBar;

    private GameState _state;
    private GameState[] _states;


    private void Start()
    {
        _states = new GameState[]
        {
            new StandartState(_scoreBar, this, _factory.StandartPrefab, _audio, _sounds),
            new CrazyState(_scoreBar, this, _factory.CrazyPrefab, _audio, _sounds),
            new LoseState(_scoreBar, this, _audio, _sounds),
            new WinState(_scoreBar, this, _audio, _sounds)
        };
        SwitchStateInternal<StandartState>(false);
    }

    private void Update()
    {
        _state.Update();
    }

    public void SwitchState<TState>() where TState : GameState
        => SwitchStateInternal<TState>(true);

    public static Vector3 GetScreenBounds()
    {
        return Camera.main.ScreenToWorldPoint(new(Screen.width, Screen.height, 0f));
    }

    public static Vector3 GetMousePos() => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    private void SwitchStateInternal<TState>(bool playSound) where TState : GameState
    {
        var state = _states.FirstOrDefault(gameState => gameState is TState);
        _state?.Freeze();
        _state = state;
#if false
        if (playSound) _audio.PlayOneShot(_sounds.StateChanged);
#endif
        state.Activate();
    }
}

public interface IStateHandler<TBaseState>
{
    void SwitchState<TState>() where TState : TBaseState;
}