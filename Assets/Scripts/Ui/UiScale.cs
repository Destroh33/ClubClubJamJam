using UnityEngine;

public static class UiScale
{
    public static float Of(Component part)
    {
        var canvas = part.GetComponentInParent<Canvas>();
        return canvas != null ? canvas.scaleFactor : 1f;
    }
}
