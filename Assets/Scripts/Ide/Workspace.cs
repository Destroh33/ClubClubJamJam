using System;
using System.Collections.Generic;

public class ScriptFile
{
    public string Name;
    public string Source;

    public ScriptFile(string name, string source)
    {
        Name = name;
        Source = source;
    }
}

public class Workspace
{
    public List<ScriptFile> Files = new List<ScriptFile>();

    public event Action Changed;

    public ScriptFile Find(string name)
    {
        foreach (var file in Files)
        {
            if (string.Equals(file.Name, name, StringComparison.OrdinalIgnoreCase))
                return file;
        }
        return null;
    }

    public void Add(ScriptFile file)
    {
        Files.Add(file);
        Raise();
    }

    public void Raise()
    {
        if (Changed != null)
            Changed();
    }

    public static Workspace CreateStarter()
    {
        var workspace = new Workspace();
        workspace.Files.Add(new ScriptFile("main", "main {\n    right(3)\n    upload(file1)\n    down(2)\n}\n"));
        workspace.Files.Add(new ScriptFile("file1", "dozer {\n    // clear a path through the trash\n    up(4)\n    useAbility()\n}\n"));
        workspace.Files.Add(new ScriptFile("file2", "waiter {\n    wait(3)\n    left(2)\n}\n"));
        return workspace;
    }
}
