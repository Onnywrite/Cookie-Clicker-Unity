using Onnywrite.Primitives;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class CrazyState : GameState
{
    // TODO: move to a scriptable object like CrazyStateSettings

    private readonly Cookie _prefab;
    // TODO: semitransparent cookies pool instead
    // Status: DONE
    private readonly Pool<Cookie> _pool;
    private readonly ThreadSafeList<Cookie> _active;
    private float _spawnProgress = 0f;
    // actually, 1 / spawnFreq that's measuring in cookies per sec
    private float _spawnDelta = 0.5f;
    private int _spawned = 0;
    private int _spawnedOverall = 0;
    private int _reclaimed = 0;


    public CrazyState(ScoreBar bar, IStateHandler<GameState> handler, Cookie cookiePrefab)
        : base(bar, handler)
    {
        _prefab = cookiePrefab;
        _pool = new(() =>
        {
            var instance = GameObject.Instantiate(_prefab, Vector3.zero, Quaternion.identity);
            RandomizeCookie(instance);
            instance.Clicked.AddListener(OnCookieClicked);
            return instance;
        }, 5);
        _active = new(5);
    }

    public override void Dispose()
    {
        _active.Clear();
        _pool.Clear();
    }

    public override void Update()
    {
        foreach (var item in _active)
        {
            if (item.transform.position.y <= -Game.GetScreenBounds().y)
            {
                scoreBar.Score -= (int)Mathf.Ceil(item.Scale);
                Debug.Log($"-{(int)Mathf.Ceil(item.Scale)} scores");
                Reclaim(item);
            }
        }

        _spawnProgress += Time.deltaTime;
        while (ShouldSpawn())
        {
            var screenBounds = Game.GetScreenBounds();
            Vector3 randVec = new(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y, 0);
            _active.Add(_pool.Take(randVec));
            _spawnProgress -= _spawnDelta;
            _spawnedOverall++;
            _spawned++;
        }
    }

    private void OnCookieClicked(Cookie clickedCookie)
    {
        Reclaim(clickedCookie);
        int scores = (int)Mathf.Clamp(1f / clickedCookie.Scale, 1f, 10f);
        Debug.Log($"+{scores} scores");
        scoreBar.Score += scores;
    }

    public override void Activate()
    {
        _spawned = 0;
        _reclaimed = 0;
    }

    public override void Freeze()
    {
    }

    private void Reclaim(Cookie cookie)
    {
        _active.Remove(cookie);
        _pool.Return(cookie);
        RandomizeCookie(cookie);
        _reclaimed++;
        if (_reclaimed == 15 && _active.Count == 0)
        {
            SwitchState<StandartState>();
        }
    }

    private void RandomizeCookie(Cookie cookie)
    {
        cookie.SetRandomPosition();
        cookie.SetRandomScale();
        cookie.SetRandomTransparency(0.4f, 0.9f);
    }

    private bool ShouldSpawn()
        => _spawnProgress >= _spawnDelta && _spawned < 15;
}