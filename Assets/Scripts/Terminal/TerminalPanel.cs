using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TerminalPanel : MonoBehaviour
{
    const int MaxChars = 24000;

    const string TextColor = "#E0E5F0";
    const string MutedColor = "#7E869C";
    const string ErrorColor = "#FF766C";
    const string GoodColor = "#8CD796";
    const string EchoColor = "#8CC8FF";
    const string AccentColor = "#FFB066";

    public TMP_InputField input;
    public TMP_Text output;
    public ScrollRect scroll;

    public Workspace Workspace;
    public LevelRunner Runner;
    public CommandRegistry Commands { get; private set; }

    public SimulationClock Clock
    {
        get { return Runner != null ? Runner.Clock : null; }
    }

    readonly StringBuilder log = new StringBuilder();
    readonly List<string> history = new List<string>();
    int historyIndex;
    string status = "";

    public void Bind(CommandRegistry commands, string greeting)
    {
        Commands = commands;

        input.onSubmit.AddListener(OnSubmit);
        input.onValueChanged.AddListener(OnTyped);
        Print(greeting);
        PrintMuted("type help to see what you can do");
        input.ActivateInputField();
    }

    void OnTyped(string typed)
    {
        if (typed.Length > 0)
            Sfx.Key();
    }

    public void SetStatus(string text)
    {
        status = text;
        Flush();
    }

    public void ClearStatus()
    {
        status = "";
        Flush();
    }

    void Update()
    {

        if (!input.isFocused || Keyboard.current == null)
            return;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            StepHistory(-1);
        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            StepHistory(1);
        if (Keyboard.current.tabKey.wasPressedThisFrame)
            Complete();
    }

    public void Print(string text)
    {
        Line(TextColor, text);
    }

    public void PrintMuted(string text)
    {
        Line(MutedColor, text);
    }

    public void PrintError(string text)
    {
        Line(ErrorColor, text);
        Sfx.Error();
    }

    public void PrintGood(string text)
    {
        Line(GoodColor, text);
    }

    public void PrintEcho(string text)
    {
        Line(EchoColor, "\n> <noparse>" + text + "</noparse>");
    }

    public void PrintHeading(string text)
    {
        Line(MutedColor, "\n" + text);
    }

    public void PrintRow(string key, string value)
    {
        log.Append("<color=").Append(AccentColor).Append(">  ").Append(key).Append("</color><pos=42%><color=").Append(MutedColor).Append('>').Append(value).Append("</color>\n");
        Flush();
    }

    public void Clear()
    {
        log.Length = 0;
        Flush();
    }

    void Line(string color, string text)
    {
        log.Append("<color=").Append(color).Append('>').Append(text).Append("</color>\n");
        Flush();
    }

    void Flush()
    {
        if (log.Length > MaxChars)
        {
            int cut = log.ToString().IndexOf('\n', log.Length / 4);
            if (cut > 0)
                log.Remove(0, cut + 1);
        }

        output.text = log.ToString() + status;
        Canvas.ForceUpdateCanvases();

        if (scroll.content.rect.height > scroll.viewport.rect.height)
            scroll.verticalNormalizedPosition = 0f;
        else
            scroll.content.anchoredPosition = Vector2.zero;
    }

    void OnSubmit(string line)
    {
        line = line.Trim();
        input.SetTextWithoutNotify("");
        input.ActivateInputField();

        if (line.Length == 0)
            return;

        Sfx.Enter();
        PrintEcho(line);
        history.Add(line);
        historyIndex = history.Count;

        var parts = line.Split(' ');
        var arguments = new string[parts.Length - 1];
        for (int i = 1; i < parts.Length; i++)
            arguments[i - 1] = parts[i];

        var command = Commands.Find(parts[0]);
        if (command == null)
        {
            PrintError("there is no command called " + parts[0]);
            return;
        }

        command.Run(arguments, this);
    }

    void StepHistory(int direction)
    {
        if (history.Count == 0)
            return;

        historyIndex = Mathf.Clamp(historyIndex + direction, 0, history.Count);
        input.SetTextWithoutNotify(historyIndex < history.Count ? history[historyIndex] : "");
        input.caretPosition = input.text.Length;
    }

    void Complete()
    {
        string typed = input.text;
        if (typed.Length == 0 || typed.Contains(" "))
            return;

        var matches = Commands.StartingWith(typed);
        if (matches.Count == 1)
        {
            input.SetTextWithoutNotify(matches[0].Name + " ");
            input.caretPosition = input.text.Length;
        }
        else if (matches.Count > 1)
        {
            var names = new List<string>();
            foreach (var match in matches)
                names.Add(match.Name);
            PrintMuted(string.Join("  ", names));
        }
    }

    public void PlayLebron() 
    {
        Sfx.Baah();
    }
}
