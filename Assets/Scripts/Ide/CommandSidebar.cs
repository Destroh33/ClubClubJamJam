using System.Collections.Generic;
using UnityEngine;

public class CommandSidebar : MonoBehaviour
{
    public RectTransform list;
    public CommandEntry entryTemplate;

    readonly List<CommandEntry> entries = new List<CommandEntry>();

    public void Build(CodePanel code)
    {
        foreach (var entry in entries)
            Destroy(entry.gameObject);
        entries.Clear();

        foreach (var command in BotCommands.All)
        {
            var entry = Instantiate(entryTemplate, list);
            entry.gameObject.SetActive(true);
            entry.Bind(code, command);
            entries.Add(entry);
        }
    }
}
