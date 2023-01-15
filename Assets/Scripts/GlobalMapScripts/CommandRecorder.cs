using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CommandRecorder : MonoBehaviour
{
    private void OnEnable()
    {
        CmdAbstract.OnExecutionEnd += OnCommandSuccessfullyFinishedExecuting;
    }
    private void OnDisable()
    {
        CmdAbstract.OnExecutionEnd -= OnCommandSuccessfullyFinishedExecuting;
    }
    SerializableDictionary<IFactionPawn, List<CmdAbstract>> commandsUnfinished;
    List<CmdAbstract> commandsAlreadyExecuted;
    public SerializableDictionary<IFactionPawn, List<CmdAbstract>> CommandsUnfinished
    { 
        get { return commandsUnfinished; } 
        private set { commandsUnfinished = value; }
    }
    public List<CmdAbstract> CommandsAlreadyExecuted
    {
        get { return commandsAlreadyExecuted; }
        private set { commandsAlreadyExecuted = value; }
    }
    private void Awake()
    {
        CommandsAlreadyExecuted = new List<CmdAbstract>();
        CommandsUnfinished = new SerializableDictionary<IFactionPawn, List<CmdAbstract>>();
    }
    public void DoThisCmd(CmdAbstract cmd)
    {
        AddCmdToUnfinishedCmdsList(cmd);
    }
    void AddCmdToUnfinishedCmdsList(CmdAbstract cmd)
    {
        if (!CommandsUnfinished.ContainsKey(cmd.factionPawn))
        {
            CommandsUnfinished.Add(cmd.factionPawn, new List<CmdAbstract>());
            CommandsUnfinished[cmd.factionPawn].Add(cmd);
            return;
        }
        CancelAllCurrentCmdsOfTheSameFactionPawn(cmd);
        CommandsUnfinished[cmd.factionPawn].Add(cmd);
        return;

        void CancelAllCurrentCmdsOfTheSameFactionPawn(CmdAbstract cmd)
        {
            while (CommandsUnfinished[cmd.factionPawn].Count>0)
            {
                CommandsUnfinished[cmd.factionPawn][0].Interrupt();
            }
        }
    }
    private void Update()
    {
        foreach (KeyValuePair<IFactionPawn, List<CmdAbstract>> plannedCmds in CommandsUnfinished)
        {
            if (plannedCmds.Value.Count > 0)
            {
                plannedCmds.Value[0].OnUpdateDelegate?.Invoke();
            }
        }
    }
    public void OnCommandSuccessfullyFinishedExecuting(CmdAbstract cmd)
    {
        CommandsUnfinished[cmd.factionPawn].Remove(cmd);
        CommandsAlreadyExecuted.Add(cmd);
        //Debug.Log(DebugCmds());
    }
    public bool IsThereAnyUnfinishedCmdsLeft()
    {
        foreach (KeyValuePair<IFactionPawn, List<CmdAbstract>> pawnCmdPair in CommandsUnfinished)
        {
            if (pawnCmdPair.Value.Count > 0)
                return true;
        }
        return false;
    }
    string DebugCmds()
    {
        return "Unfinished: " + CommandsUnfinished.Count + " Finished: " + CommandsAlreadyExecuted.Count + "\n" +
            DebugUnfinishedCmds() + "\n" + DebugFinishedCmds();
    }
    string DebugUnfinishedCmds()
    {
        string s = "";
        foreach (KeyValuePair<IFactionPawn, List<CmdAbstract>> plannedCmds in CommandsUnfinished)
        {
            s += plannedCmds.Key+": ";
            foreach (CmdAbstract cmd in plannedCmds.Value)
            {
                s += cmd + ",";
            }
            s += "\n";
        }
        return s;
    }
    string DebugFinishedCmds()
    {
        string s = "";
        foreach (CmdAbstract cmd in CommandsAlreadyExecuted)
        {
            s += cmd + ",";
        }
        return s;
    }

}


