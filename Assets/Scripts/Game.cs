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
// boss (couple clicks to kill one coockie
// etc.
// Status: Almost done
public class Game : MonoBehaviour, IStateHandler<GameState>
{
    // TODO: implement ObjectFactory instead
    // Status: DONE
    [SerializeField]
    private ObjectFactory _factory;

    [SerializeField]
    private ScoreBar _scoreBar;

    private GameState _state;
    private GameState[] _states;


    private void Start()
    {
        _states = new GameState[]
        {
            new StandartState(_scoreBar, this, _factory.StandartPrefab),
            new CrazyState(_scoreBar, this, _factory.CrazyPrefab)
        };
        SwitchState<StandartState>();
    }

    private void Update()
    {
        _state.Update();
    }

    public void SwitchState<TState>() where TState : GameState
    {
        var state = _states.FirstOrDefault(gameState => gameState is TState);
        _state?.Freeze();
        _state = state;
        state.Activate();
    }

    public static Vector3 GetScreenBounds()
    {
        return Camera.main.ScreenToWorldPoint(new(Screen.width, Screen.height, 0f));
    }
}

public interface IStateHandler<TBaseState>
{
    void SwitchState<TState>() where TState : TBaseState;
}