using UnityEngine;

/// <summary>
/// Tween GameObject's local scale.
/// </summary>
[AddComponentMenu("ZTween/Tween Anchors")]
public class TweenAnchors : TweenBase
{
    public Rect from = new Rect();
    public Rect to = new Rect();

    private Rect rect = new Rect();
    private RectTransform rectTransform;

    public Rect value
    {
        get
        {
            if (rectTransform == null)
                rectTransform = transform as RectTransform;
            rect.position = rectTransform.anchorMin;
            rect.size = rectTransform.anchorMax;
            return rect;
        }
        set
        {
            if (rectTransform == null)
                rectTransform = transform as RectTransform;
            rectTransform.anchorMin = value.position;
            rectTransform.anchorMax = value.size;
        }
    }

    /// <summary>
    /// Call Tweening Logic.
    /// </summary>
    /// <param name="factor">Factor.</param>
    /// <param name="isFinished">If set to <c>true</c> is finished.</param>
    protected override void OnUpdate(float factor, bool isFinished)
    {
        rect.position = from.position * (1f - factor) + to.position * factor;
        rect.size = from.size * (1f - factor) + to.size * factor;
        value = rect;
    }

    /// <summary>
    /// Begin tweening operation.
    /// </summary>
    /// <param name="scale">Final Anchors value</param>
    static public TweenAnchors Begin(GameObject go, float duration, Rect anchors)
    {
        TweenAnchors comp = TweenBase.Begin<TweenAnchors>(go, duration);
        comp.from = comp.value;
        comp.to = anchors;

        if (duration <= 0f)
        {
            comp.OnFactorModified(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    /// <summary>
    /// Set 'from' value to current value.
    /// </summary>
    [ContextMenu("Set 'From' to current value")]
    public override void SetStartToCurrentValue()
    {
        from = value;
    }

    /// <summary>
    /// Set 'to' value to the current value.
    /// </summary>
    [ContextMenu("Set 'To' to current value")]
    public override void SetEndToCurrentValue()
    {
        to = value;
    }

    /// <summary>
    /// Sets current value to 'from' value.
    /// </summary>
    [ContextMenu("Assume value of 'From'")]
    public override void SetCurrentValueToStart()
    {
        value = from;
    }

    /// <summary>
    /// Sets current value to 'to' value.
    /// </summary>
    [ContextMenu("Assume value of 'To'")]
    public override void SetCurrentValueToEnd()
    {
        value = to;
    }
}
