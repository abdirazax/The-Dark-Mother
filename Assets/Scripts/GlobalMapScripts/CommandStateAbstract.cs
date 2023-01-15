/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public abstract class CommandStateAbstract
{
    public abstract void StartState(CmdAbstract cmd);
    public abstract void LateUpdateState(CmdAbstract cmd);
    public abstract void EndState(CmdAbstract cmd);
}
public class CommandStateNotStarted : CommandStateAbstract
{
    public override void StartState(CmdAbstract cmd) { }
    public override void EndState(CmdAbstract cmd)
    {
        cmd.DoWhenStartedExecuting();
        cmd.CmdCurrentState = cmd.cmdStateExecuting;
        cmd.CmdCurrentState.StartState(cmd);
    }
    public override void LateUpdateState(CmdAbstract cmd) { }
}
public class CommandStateStartedExecuting : CommandStateAbstract
{
    public override void StartState(CmdAbstract cmd) { }
    public override void LateUpdateState(CmdAbstract cmd)
    {
        if (cmd.IsFinishConditionMet())
        {
            EndState(cmd);
            return;
        }
    }
    public override void EndState(CmdAbstract cmd)
    {
        cmd.CmdCurrentState = cmd.cmdFinishedExecuting;
        cmd.CmdCurrentState.StartState(cmd);
    }
}
public class CommandStateFinishedExecuting : CommandStateAbstract
{
    public override void StartState(CmdAbstract cmd)
    {
        cmd.DoWhenEndedExecuting();
    }
    public override void EndState(CmdAbstract cmd) { }
    public override void LateUpdateState(CmdAbstract cmd) { }
}
*/