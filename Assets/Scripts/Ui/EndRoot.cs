using UnityEngine;

public class EndRoot : MonoBehaviour
{
    public TerminalPanel terminal;
    public string tagline = "every shell accounted for";
    public string artSpacing = "0.5em";
    public string menuScene = "MenuScene";

    public LevelConfig Offer { get; private set; }

    void Start()
    {
        Offer = GameSession.Offer;
        GameSession.Offer = null;

        EndCommands.End = this;

        var registry = new CommandRegistry();
        EndCommands.RegisterAll(registry);

        string art = "<mspace=" + artSpacing + ">" + CrabArt.Win + "</mspace>";
        terminal.Bind(registry, art + "\n" + tagline, false);

        if (Offer != null)
            terminal.Print("there is a secret level. do you want to play it? (y or n)");
    }

    public void Play(LevelConfig level)
    {
        GameSession.Selected = level;
        SceneFade.Go(level.sceneName.Length > 0 ? level.sceneName : menuScene);
    }

    public void Menu()
    {
        SceneFade.Go(menuScene);
    }
}
