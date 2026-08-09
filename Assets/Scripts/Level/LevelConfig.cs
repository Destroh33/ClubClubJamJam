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
    public LevelConfig next;

    public Workspace CreateWorkspace()
    {
        var workspace = new Workspace();
        workspace.Files.Add(new ScriptFile("main", mainProgram));

        for (int i = 0; i < extraFiles; i++)
        {
            string source = i < extraPrograms.Count ? extraPrograms[i] : "";
            workspace.Files.Add(new ScriptFile("file" + (i + 1), source));
        }

        return workspace;
    }
}
