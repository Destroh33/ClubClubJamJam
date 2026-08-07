using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public WindowPanel window;
    public int xDir;
    public int yDir;

    bool dragging;

    public void OnBeginDrag(PointerEventData pointer)
    {
        dragging = true;
    }

    public void OnDrag(PointerEventData pointer)
    {
        window.Resize(xDir, yDir, pointer.delta / UiScale.Of(this));
    }

    public void OnEndDrag(PointerEventData pointer)
    {
        dragging = false;
        GameRoot.Cursors.ShowPointer();
    }

    public void OnPointerEnter(PointerEventData pointer)
    {
        GameRoot.Cursors.ShowResize(Direction());
    }

    public void OnPointerExit(PointerEventData pointer)
    {
        if (!dragging)
            GameRoot.Cursors.ShowPointer();
    }

    ResizeDirection Direction()
    {
        if (xDir != 0 && yDir != 0)
            return xDir * yDir < 0 ? ResizeDirection.DiagonalDown : ResizeDirection.DiagonalUp;
        return xDir != 0 ? ResizeDirection.Horizontal : ResizeDirection.Vertical;
    }
}
