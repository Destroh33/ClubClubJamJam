using UnityEngine;
using UnityEngine.EventSystems;

public class TitleBar : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public WindowPanel window;

    bool dragging;

    public void OnBeginDrag(PointerEventData pointer)
    {
        dragging = true;
    }

    public void OnDrag(PointerEventData pointer)
    {
        window.Move(pointer.delta / UiScale.Of(this));
    }

    public void OnEndDrag(PointerEventData pointer)
    {
        dragging = false;
        GameRoot.Cursors.ShowPointer();
    }

    public void OnPointerClick(PointerEventData pointer)
    {
        if (pointer.clickCount >= 2)
            window.ToggleMaximize();
    }

    public void OnPointerEnter(PointerEventData pointer)
    {
        GameRoot.Cursors.ShowHand();
    }

    public void OnPointerExit(PointerEventData pointer)
    {
        if (!dragging)
            GameRoot.Cursors.ShowPointer();
    }
}
