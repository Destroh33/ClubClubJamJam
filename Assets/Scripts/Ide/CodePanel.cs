using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CodePanel : MonoBehaviour
{
    const int MaxHistory = 200;

    public TMP_InputField input;
    public TMP_Text overlay;
    public TMP_Text gutter;
    public TMP_Text status;
    public Button tabTemplate;
    public SyntaxTheme theme = new SyntaxTheme();

    Workspace workspace;
    ScriptFile current;
    readonly List<Button> tabs = new List<Button>();
    readonly List<string> history = new List<string>();
    int historyIndex;

    public void Bind(Workspace files)
    {
        workspace = files;
        input.textComponent.color = Color.clear;
        input.onValueChanged.AddListener(OnEdited);
        BuildTabs();
        Open(workspace.Files[0]);
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (!input.isFocused || keyboard == null || !keyboard.ctrlKey.isPressed)
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
        input.SetTextWithoutNotify(file.Source);
        history.Clear();
        history.Add(file.Source);
        historyIndex = 0;
        Refresh();
        Recolor();
    }

    void OnEdited(string value)
    {
        current.Source = value;

        history.RemoveRange(historyIndex + 1, history.Count - historyIndex - 1);
        history.Add(value);
        if (history.Count > MaxHistory)
            history.RemoveAt(0);
        historyIndex = history.Count - 1;

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

    void Refresh()
    {
        string source = current.Source;
        overlay.text = SyntaxHighlighter.Highlight(source, theme);
        gutter.text = BuildGutter(source);
    }

    string BuildGutter(string source)
    {
        int count = source.Split('\n').Length;
        var text = new StringBuilder();
        for (int line = 1; line <= count; line++)
        {
            if (line > 1)
                text.Append('\n');
            text.Append(line);
        }
        return text.ToString();
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
