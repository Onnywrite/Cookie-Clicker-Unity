using Onnywrite.Common;
using UnityEngine;

public sealed class StandartState : GameState
{
    private readonly Cookie _cookiePrefab;
    private readonly Cookie _cookie;
    private int _clicked = 0;

    public StandartState(ScoreBar bar, IStateHandler<GameState> handler,
        Cookie cookiePrefab, AudioSource asource, AudioFactory sounds)
        : base(bar, handler, asource, sounds)
    {
        _cookiePrefab = cookiePrefab;
        _cookie = GameObject.Instantiate(_cookiePrefab, Vector3.zero, Quaternion.identity);
        _cookie.gameObject.SetActive(false);
    }

    public override void Dispose()
    {
        GameObject.Destroy(_cookie);
    }

    private void OnClickAnimationEnded(Cookie cookie)
    {
        cookie.SetRandomPosition();
        cookie.SetRandomScale();
        if (scoreBar.Score >= 1309) SwitchState<WinState>();
        if (_clicked == 10) SwitchState<CrazyState>();
    }

    public override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_cookie.TryClick(Game.GetMousePos(), OnClickAnimationEnded))
            {
                //audio.PlayOneShot(sounds.PlusScore);
                if (Random.Range(1, 11) != 1) audio.PlayOneShot(sounds.Clicks.Pick());
                else audio.PlayOneShot(sounds.DataTransfering);
                scoreBar.Increment();
                _clicked++;
            }
        }
    }

    public override void Activate()
    {
        _clicked = 0;
        _cookie.gameObject.SetActive(true);
    }

    public override void Freeze()
    {
        _cookie.gameObject.SetActive(false);
    }
}
