using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CodePanel : MonoBehaviour
{
    const int MaxHistory = 200;
    const string Indent = "    ";
    const string TabIndent = "   ";

    public TMP_InputField input;
    public TMP_Text overlay;
    public TMP_Text gutter;
    public TMP_Text status;
    public Button tabTemplate;
    public CommandSidebar sidebar;
    public RectTransform dropLine;
    public SyntaxTheme theme = new SyntaxTheme();

    Workspace workspace;
    ScriptFile current;
    readonly List<Button> tabs = new List<Button>();
    readonly List<string> history = new List<string>();
    int historyIndex;
    int previewLine = -1;

    public void Bind(Workspace files)
    {
        workspace = files;
        input.textComponent.color = Color.clear;
        input.onValueChanged.AddListener(OnEdited);

        if (dropLine != null)
        {
            dropLine.GetComponent<Image>().color = theme.dropLine;
            dropLine.gameObject.SetActive(false);
        }

        BuildTabs();
        Open(workspace.Files[0]);

        if (sidebar != null)
            sidebar.Build(this);
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (!input.isFocused || keyboard == null)
            return;

        if (keyboard.tabKey.wasPressedThisFrame)
        {
            InsertAtCaret(TabIndent);
            return;
        }

        if (!keyboard.ctrlKey.isPressed)
            return;

        if (keyboard.zKey.wasPressedThisFrame)
            Undo();
        if (keyboard.yKey.wasPressedThisFrame)
            Redo();
    }

    void LateUpdate()
    {
        var text = input.textComponent.rectTransform;
        var numbers = gutter.rectTransform;
        numbers.anchoredPosition = new Vector2(numbers.anchoredPosition.x, text.anchoredPosition.y);
    }

    void Open(ScriptFile file)
    {
        current = file;
        previewLine = -1;
        file.Source = Untab(file.Source);
        input.SetTextWithoutNotify(file.Source);
        history.Clear();
        history.Add(file.Source);
        historyIndex = 0;
        Refresh();
        Recolor();
    }

    void OnEdited(string value)
    {
        if (value.IndexOf('\t') >= 0)
        {
            value = Untab(value);
            input.SetTextWithoutNotify(value);
        }

        Record(value);
        Refresh();
    }

    void Record(string value)
    {
        current.Source = value;

        history.RemoveRange(historyIndex + 1, history.Count - historyIndex - 1);
        history.Add(value);
        if (history.Count > MaxHistory)
            history.RemoveAt(0);
        historyIndex = history.Count - 1;
    }

    void InsertAtCaret(string text)
    {
        int at = Mathf.Clamp(input.caretPosition, 0, current.Source.Length);
        Write(current.Source.Insert(at, text), at + text.Length);
    }

    void Write(string value, int caret)
    {
        Record(value);
        input.SetTextWithoutNotify(value);
        input.caretPosition = Mathf.Clamp(caret, 0, value.Length);
        input.stringPosition = input.caretPosition;
        Refresh();
    }

    void Undo()
    {
        if (historyIndex > 0)
            Restore(historyIndex - 1);
    }

    void Redo()
    {
        if (historyIndex < history.Count - 1)
            Restore(historyIndex + 1);
    }

    void Restore(int index)
    {
        historyIndex = index;
        current.Source = history[index];
        input.SetTextWithoutNotify(current.Source);
        input.caretPosition = Mathf.Min(input.caretPosition, current.Source.Length);
        Refresh();
    }

    public bool PointerOverCode(Vector2 screenPosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(input.textViewport, screenPosition, null);
    }

    public void ShowDropPreview(Vector2 screenPosition)
    {
        int line = LineAt(screenPosition);
        if (line == previewLine)
            return;

        previewLine = line;
        Refresh();
    }

    public void HideDropPreview()
    {
        if (previewLine < 0)
            return;

        previewLine = -1;
        Refresh();
    }

    public void DropCommand(string text)
    {
        if (previewLine < 0)
            return;

        int line = previewLine;
        previewLine = -1;

        string source = current.Source;
        int at = LineStart(source, line);
        string added = IndentAbove(source, line) + text + "\n";

        Write(source.Insert(at, added), at + added.Length - 1);
        input.ActivateInputField();
    }

    int LineAt(Vector2 screenPosition)
    {
        int lines = CountLines(current.Source);
        int line = TMP_TextUtilities.FindNearestLine(overlay, screenPosition, null);

        if (line < 0)
            return Mathf.Max(0, lines - 1);

        if (previewLine >= 0 && line > previewLine)
            line--;

        return Mathf.Clamp(line, 0, Mathf.Max(0, lines - 1));
    }

    string IndentAbove(string source, int line)
    {
        int at = LineStart(source, line);

        for (int above = line - 1; above >= 0; above--)
        {
            int start = LineStart(source, above);
            int end = source.IndexOf('\n', start);
            if (end < 0 || end > at)
                end = at;

            string text = source.Substring(start, end - start);
            if (text.Trim().Length == 0)
                continue;

            string spaces = text.Substring(0, text.Length - text.TrimStart(' ').Length);
            return text.TrimEnd().EndsWith("{") ? spaces + Indent : spaces;
        }

        return Indent;
    }

    static int LineStart(string source, int line)
    {
        int at = 0;
        for (int i = 0; i < line; i++)
        {
            int next = source.IndexOf('\n', at);
            if (next < 0)
                return source.Length;
            at = next + 1;
        }
        return at;
    }

    static int CountLines(string source)
    {
        int count = 1;
        for (int i = 0; i < source.Length; i++)
        {
            if (source[i] == '\n')
                count++;
        }
        return count;
    }

    static string Untab(string source)
    {
        return source.IndexOf('\t') >= 0 ? source.Replace("\t", Indent) : source;
    }

    void Refresh()
    {
        string source = current.Source;
        string shown = previewLine >= 0 ? source.Insert(LineStart(source, previewLine), "\n") : source;

        overlay.text = SyntaxHighlighter.Highlight(shown, theme);
        gutter.text = BuildGutter(shown);
        PlaceDropLine();
    }

    string BuildGutter(string shown)
    {
        int count = CountLines(shown);
        int number = 1;
        var text = new StringBuilder();

        for (int line = 0; line < count; line++)
        {
            if (line > 0)
                text.Append('\n');
            if (line == previewLine)
                continue;
            text.Append(number);
            number++;
        }

        return text.ToString();
    }

    void PlaceDropLine()
    {
        if (dropLine == null)
            return;

        if (previewLine < 0)
        {
            dropLine.gameObject.SetActive(false);
            return;
        }

        overlay.ForceMeshUpdate();
        var info = overlay.textInfo;
        if (previewLine >= info.lineCount)
        {
            dropLine.gameObject.SetActive(false);
            return;
        }

        var line = info.lineInfo[previewLine];
        dropLine.gameObject.SetActive(true);
        dropLine.anchoredPosition = new Vector2(0f, (line.ascender + line.descender) * 0.5f);
        dropLine.sizeDelta = new Vector2(0f, line.ascender - line.descender);
    }

    void BuildTabs()
    {
        foreach (var tab in tabs)
            Destroy(tab.gameObject);
        tabs.Clear();

        foreach (var file in workspace.Files)
        {
            var target = file;
            var tab = Instantiate(tabTemplate, tabTemplate.transform.parent);
            tab.gameObject.SetActive(true);
            tab.GetComponentInChildren<TMP_Text>().text = file.Name;
            tab.onClick.AddListener(() =>
            {
                Open(target);
                Recolor();
            });
            tabs.Add(tab);
        }
    }

    void Recolor()
    {
        for (int i = 0; i < tabs.Count; i++)
        {
            var label = tabs[i].GetComponentInChildren<TMP_Text>();
            label.color = workspace.Files[i] == current ? theme.programName : theme.comment;
        }
    }
}
