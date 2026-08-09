using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BrainRobot : Robot
{
    private TerminalPanel terminal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        terminal = FindAnyObjectByType<TerminalPanel>();
    }

    public override void UseAbility()
    {
        Debug.Log("ability used");
    }

    public override void AttachScript()
    {
        Debug.Log("attach script");
    }


    /// <summary>
    /// note that file 0 is the brain file used for this robot - probably won't work properly for child bots
    /// </summary>
    /// <param name="otherBot"></param>
    /// <param name="numOfFile"></param>
    public void UploadToBot(CodeExecutor otherBot, int numOfFile) 
    {
        Workspace workspace = terminal.Workspace;
        ScriptFile file = workspace.Files[numOfFile];
        List<CommandsListEntry> comList = CodeParsing.ParseText(file.Source);
        otherBot.SetCommandList(comList);
    }

    public void OnAttack()
    {
        UseAbility();
    }
}
