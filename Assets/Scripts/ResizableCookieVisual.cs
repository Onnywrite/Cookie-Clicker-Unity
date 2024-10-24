using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ResizableCookieVisual : CookieVisualBase
{
    [SerializeField]
    protected SpriteRenderer mainRenderer;

    protected Animator animator;
    protected int clickedAnimHash;

    //private Rect _maxSize;
    //private CachedSprite _sprite;

    public virtual Color SpriteColor
    {
        get => mainRenderer.color;
        set => mainRenderer.color = value;
    }
    public override Color Color { get => mainRenderer.color; set => mainRenderer.color = value; }
    public override bool IsBeingClicked { get; protected set; }

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        clickedAnimHash = Animator.StringToHash("Clicked");
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
        IsBeingClicked = false;
    }