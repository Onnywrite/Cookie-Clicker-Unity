using UnityEngine;

public sealed class StandartState : GameState
{
    private readonly Cookie _cookiePrefab;
    private readonly Cookie _cookie;
    private int _clicked = 0;

    public StandartState(ScoreBar bar, IStateHandler<GameState> handler, Cookie cookiePrefab)
        : base(bar, handler)
    {
        _cookiePrefab = cookiePrefab;
        _cookie = GameObject.Instantiate(_cookiePrefab, Vector3.zero, Quaternion.identity);
        _cookie.gameObject.SetActive(false);
        _cookie.Clicked.AddListener(OnCookieClicked);
    }

    public override void Dispose()
    {
        GameObject.Destroy(_cookie);
    }

    private void OnCookieClicked(Cookie clickedCookie)
    {
        scoreBar.Score++;
        _clicked++;
        if (_clicked == 10) 
        {
            SwitchState<CrazyState>();
        }
        _cookie.SetRandomPosition();
        _cookie.SetRandomScale();
    }

    public override void Update()
    {

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
