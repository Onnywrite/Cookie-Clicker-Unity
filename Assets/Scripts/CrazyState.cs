using Onnywrite.Common;
using Onnywrite.Primitives;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class CrazyState : GameState
{
    private readonly Cookie _prefab;
    private readonly Pool<Cookie> _pool;
    private readonly ThreadSafeList<Cookie> _active;
    private float _spawnProgress = 0f;
    // actually, 1 / spawnFreq that's measuring in cookies per sec
    private float _spawnDelta = 1f; // legacy: 0.5f
    private int _spawned = 0;
    private int _spawnedOverall = 0;
    private int _reclaimed = 0;

    private float _penaltyScoreK = 1f;
    private int _maxSpawned = 15;
    private int _loseScorePower = 2;


    public CrazyState(ScoreBar bar, IStateHandler<GameState> handler,
        Cookie cookiePrefab, AudioSource asource, AudioFactory sounds)
        : base(bar, handler, asource, sounds)
    {
        _prefab = cookiePrefab;
        _pool = new(() =>
        {
            var instance = GameObject.Instantiate(_prefab, Vector3.zero, Quaternion.identity);
            instance.Randomize();
            instance.Clicked.AddListener(OnCookieClicked);
            instance.ApplyGravity(true);
            return instance;
        }, 5);
        _active = new(5);
    }

    public float PenaltyScoreK { get => _penaltyScoreK; private set => _penaltyScoreK = Mathf.Max(value, 1f); }

    public float SpawnDelta { get => _spawnDelta; private set => _spawnDelta = Mathf.Max(value, 0.2f); }

    public override void Dispose()
    {
        _active.Clear();
        _pool.Clear();
    }

    public override void Update()
    {
        _spawnProgress += Time.deltaTime;
        var screenBounds = Game.GetScreenBounds();
        foreach (var item in _active)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!item.TryClick(Game.GetMousePos(), Reclaim)) PenaltyScoreK += 0.1f;
            }

            if (!MathGeek.InRange(item.transform.position.x, -screenBounds.x, screenBounds.x))
            {
                if (item.TryGetComponent<Rigidbody2D>(out var rb))
                    rb.AddForce(-item.transform.localPosition);
            }
            else if (item.transform.position.y <= -Game.GetScreenBounds().y)
            {
                int scores = (int)Mathf.Ceil(Mathf.Pow(item.Scale, _loseScorePower) * PenaltyScoreK);
                PenaltyScoreK -= 0.2f;
                scoreBar.AddScore(-scores);
                Debug.Log($"-{scores} scores");
                audio.PlayOneShot(sounds.MinusScore, 0.8f);
                Reclaim(item);
                if (scoreBar.Score < 0) SwitchState<LoseState>();
            }
        }

        while (ShouldSpawn())
        {
            Vector3 randVec = new(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y, 0);
            var newCookie = _pool.Take(randVec);
            newCookie.ApplyGravity();
            _active.Add(newCookie);
            _spawnProgress -= SpawnDelta;
            _spawnedOverall++;
            _spawned++;
        }
    }

    private void OnCookieClicked(Cookie clickedCookie)
    {
        clickedCookie.ApplyGravity(false);

        int scores = (int)Mathf.Clamp(1f / clickedCookie.Scale, 1f, 10f);
        audio.PlayOneShot(sounds.Clicks.Pick());
        scoreBar.AddScore(scores);
    }

    public override void Activate()
    {
        _maxSpawned += _spawnedOverall % 250 == 0 ? 1 : 0;
        SpawnDelta -= 0.025f;
        if (scoreBar.Score >= 500) _loseScorePower = 3;
        else _loseScorePower = 2;
        _spawned = 0;
        _reclaimed = 0;
    }

    public override void Freeze()
    {
    }

    private void Reclaim(Cookie cookie)
    {
        if (scoreBar.Score >= 1309) SwitchState<WinState>();
        _active.Remove(cookie);
        _pool.Return(cookie);
        cookie.Randomize();
        _reclaimed++;
        if (_reclaimed == _maxSpawned && _active.Count == 0)
        {
            SwitchState<StandartState>();
        }
    }

    private bool ShouldSpawn()
        => _spawnProgress >= SpawnDelta && _spawned < _maxSpawned;
}