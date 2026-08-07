using System;
using UnityEngine;

public enum ResizeDirection
{
    Horizontal,
    Vertical,
    DiagonalDown,
    DiagonalUp
}

[Serializable]
public class CursorArt
{
    public Texture2D texture;
    public Vector2 hotspot;
}

[Serializable]
public class CursorSet
{
    public CursorArt pointer = new CursorArt();
    public CursorArt hand = new CursorArt();
    public CursorArt resizeHorizontal = new CursorArt();
    public CursorArt resizeVertical = new CursorArt();
    public CursorArt resizeDiagonalDown = new CursorArt();
    public CursorArt resizeDiagonalUp = new CursorArt();

    public void ShowPointer()
    {
        Show(pointer);
    }

    public void ShowHand()
    {
        Show(hand);
    }

    public void ShowResize(ResizeDirection direction)
    {
        Show(ForResize(direction));
    }

    public void Show(CursorArt art)
    {
        if (art == null || art.texture == null)
            return;
        Cursor.SetCursor(art.texture, art.hotspot, CursorMode.Auto);
    }

    CursorArt ForResize(ResizeDirection direction)
    {
        switch (direction)
        {
            case ResizeDirection.Vertical:
                return resizeVertical;
            case ResizeDirection.DiagonalDown:
                return resizeDiagonalDown;
            case ResizeDirection.DiagonalUp:
                return resizeDiagonalUp;
            default:
                return resizeHorizontal;
        }
    }
}
