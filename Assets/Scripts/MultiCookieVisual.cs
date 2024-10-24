using Onnywrite.Common;
using System;
using System.Collections;
using UnityEngine;

public class MultiCookieVisual : ResizableCookieVisual
{
    [SerializeField]
    private SpriteRenderer[] _spritesPrefabs;

    private SpriteRenderer[] _sprites;

    private void Awake()
    {
        _sprites = new SpriteRenderer[_spritesPrefabs.Length];
        for (int i = 0; i < _spritesPrefabs.Length; i++)
        {
            _sprites[i] = Instantiate(_spritesPrefabs[i], Vector3.zero, Quaternion.identity, transform);
            _sprites[i].gameObject.SetActive(false);
        }
        SetRandomRenderer();
    }

    public override void Click(Action<Cookie> clickedCallback, Cookie instance)
    {
        if (IsBeingClicked) return;
        IsBeingClicked = true;
        animator.SetTrigger(clickedAnimHash);
        StartCoroutine(WaitUntilFinalAnimEnded(clickedCallback, instance));
    }
    private IEnumerator WaitUntilFinalAnimEnded(Action<Cookie> clickedCallback, Cookie instance)
    {
        var len = animator.GetCurrentAnimatorStateInfo(0).length * 0.8f;
        yield return new WaitForSeconds(len);
        clickedCallback(instance);
        SetRandomRenderer();
        IsBeingClicked = false;
    }

    private void SetRandomRenderer()
    {
        mainRenderer.gameObject.SetActive(false);
        mainRenderer = _sprites.Pick();
        mainRenderer.gameObject.SetActive(true);
    }
}
