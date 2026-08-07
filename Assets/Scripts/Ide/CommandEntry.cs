using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandEntry : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image background;
    public TMP_Text label;
    public Color idleColor = new Color(0.13f, 0.15f, 0.2f, 1f);
    public Color hoverColor = new Color(0.19f, 0.22f, 0.29f, 1f);

    CodePanel code;
    string text;
    TMP_Text ghost;
    RectTransform ghostParent;

    public void Bind(CodePanel panel, CommandInfo command)
    {
        code = panel;
        text = command.Usage;
        label.text = SyntaxHighlighter.Highlight(text, panel.theme);
        background.color = idleColor;
    }

    public void OnPointerEnter(PointerEventData pointer)
    {
        background.color = hoverColor;
        GameRoot.Cursors.ShowHand();
    }

    public void OnPointerExit(PointerEventData pointer)
    {
        background.color = idleColor;
        GameRoot.Cursors.ShowPointer();
    }

    public void OnBeginDrag(PointerEventData pointer)
    {
        var canvas = GetComponentInParent<Canvas>().rootCanvas;
        ghostParent = (RectTransform)canvas.transform;
        ghost = Instantiate(label, ghostParent);
        ghost.raycastTarget = false;
        ghost.alpha = 0.85f;

        var rect = ghost.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.zero;
        rect.pivot = new Vector2(0.5f, 0f);
        rect.sizeDelta = label.rectTransform.rect.size;
        rect.SetAsLastSibling();

        Follow(pointer);
    }

    public void OnDrag(PointerEventData pointer)
    {
        if (ghost == null)
            return;

        Follow(pointer);

        if (code.PointerOverCode(pointer.position))
            code.ShowDropPreview(pointer.position);
        else
            code.HideDropPreview();
    }

    public void OnEndDrag(PointerEventData pointer)
    {
        if (ghost != null)
            Destroy(ghost.gameObject);
        ghost = null;
        ghostParent = null;

        if (code.PointerOverCode(pointer.position))
        {
            code.ShowDropPreview(pointer.position);
            code.DropCommand(text);
        }
        else
            code.HideDropPreview();
    }

    void Follow(PointerEventData pointer)
    {
        Vector2 local;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(ghostParent, pointer.position, pointer.pressEventCamera, out local))
            return;

        ghost.rectTransform.anchoredPosition = local - ghostParent.rect.min;
    }
}
