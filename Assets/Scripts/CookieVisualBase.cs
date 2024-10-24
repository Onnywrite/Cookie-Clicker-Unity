using System;
using UnityEngine;

public abstract class CookieVisualBase : MonoBehaviour
{
    public abstract Color Color { get; set; }
    public abstract bool IsBeingClicked { get; protected set; }
    public abstract void Click(Action<Cookie> clickedCallback, Cookie instance);
    //internal abstract void __StopAnimate__();
    //public abstract Rect MaxSize { get; set; }
}