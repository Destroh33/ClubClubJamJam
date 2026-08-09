using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteAlways]
public class WindowPanel : MonoBehaviour, IPointerDownHandler
{
    const float MinWidth = 280f;
    const float MinHeight = 160f;

    public bool locked;

    public bool Maximized { get; private set; }

    Vector2 savedPosition;
    Vector2 savedSize;

    RectTransform Rect
    {
        get { return (RectTransform)transform; }
    }

    RectTransform Parent
    {
        get { return (RectTransform)transform.parent; }
    }

    public void OnPointerDown(PointerEventData pointer)
    {
        transform.SetAsLastSibling();
    }

    void Start()
    {
        if (locked)
            Maximize();
    }

    void LateUpdate()
    {
        if (locked)
            Rect.sizeDelta = Parent.rect.size;
    }

    public void ToggleMaximize()
    {
        if (locked)
            return;

        if (Maximized)
        {
            Rect.anchoredPosition = savedPosition;
            Rect.sizeDelta = savedSize;
            Maximized = false;
            transform.SetAsLastSibling();
            return;
        }

        Maximize();
    }

    void Maximize()
    {
        savedPosition = Rect.anchoredPosition;
        savedSize = Rect.sizeDelta;
        Rect.anchoredPosition = Vector2.zero;
        Rect.sizeDelta = Parent.rect.size;
        Maximized = true;
        transform.SetAsLastSibling();
    }

    public void Move(Vector2 delta)
    {
        if (Maximized)
            return;

        var position = Rect.anchoredPosition + delta;
        position.x = Mathf.Clamp(position.x, 0f, Mathf.Max(0f, Parent.rect.width - Rect.sizeDelta.x));
        position.y = Mathf.Clamp(position.y, Mathf.Min(0f, Rect.sizeDelta.y - Parent.rect.height), 0f);
        Rect.anchoredPosition = position;
    }

    public void Resize(int xDir, int yDir, Vector2 delta)
    {
        if (Maximized)
            return;

        var size = Rect.sizeDelta;
        var position = Rect.anchoredPosition;

        if (xDir > 0)
            size.x += delta.x;
        else if (xDir < 0)
        {
            size.x -= delta.x;
            position.x += delta.x;
        }

        if (yDir > 0)
        {
            size.y += delta.y;
            position.y += delta.y;
        }
        else if (yDir < 0)
            size.y -= delta.y;

        float width = Mathf.Max(size.x, MinWidth);
        if (xDir < 0)
            position.x -= width - size.x;
        size.x = width;

        float height = Mathf.Max(size.y, MinHeight);
        if (yDir > 0)
            position.y += height - size.y;
        size.y = height;

        Rect.sizeDelta = size;
        Rect.anchoredPosition = position;
        Move(Vector2.zero);
    }
}
