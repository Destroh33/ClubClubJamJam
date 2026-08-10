using UnityEngine;

public class MenuRoot : MonoBehaviour
{
    public TerminalPanel terminal;
    public string tagline = "a game about securing shells";
    public int artGap = 6;
    public string artSpacing = "0.5em";
    public LevelConfig[] levels;
    public string levelScene = "SampleScene";
    public string[] credits = { "a game about crabs and code" };

    void Start()
    {
        MenuCommands.Menu = this;

        var registry = new CommandRegistry();
        MenuCommands.RegisterAll(registry);
        string art = "<mspace=" + artSpacing + ">" + CrabArt.Banner(artGap) + "</mspace>";
        terminal.Bind(registry, art + "\n" + tagline);
    }

    public void Load(LevelConfig level)
    {
        GameSession.Selected = level;
        SceneFade.Go(level.sceneName.Length > 0 ? level.sceneName : levelScene);
    }
}
