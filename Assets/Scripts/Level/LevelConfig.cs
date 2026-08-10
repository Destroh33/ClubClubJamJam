using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shellsweep/Level")]
public class LevelConfig : ScriptableObject
{
    public string levelName = "level";
    [TextArea(4, 12)] public string mainProgram = "main {\n\n}\n";
    public int extraFiles = 2;
    public List<string> extraPrograms = new List<string>();
    public float startSpeed = 1f;
    public int maxTicks = 40;
    public LevelConfig next;

    public Workspace CreateWorkspace()
    {
        var workspace = new Workspace();
        workspace.Files.Add(new ScriptFile("main", Wrap("main", mainProgram)));

        for (int i = 0; i < extraFiles; i++)
        {
            string name = "file" + (i + 1);
            string body = i < extraPrograms.Count ? extraPrograms[i] : "";
            workspace.Files.Add(new ScriptFile(name, Wrap(name, body)));
        }

        return workspace;
    }

    static string Wrap(string name, string body)
    {
        var text = new System.Text.StringBuilder();
        text.Append(name).Append(" {\n");

        foreach (string line in body.Split('\n'))
        {
            string trimmed = line.Trim();
            if (trimmed == "}" || trimmed.EndsWith("{"))
                continue;

            text.Append(line).Append('\n');
        }

        text.Append('}');
        return text.ToString();
    }
}
