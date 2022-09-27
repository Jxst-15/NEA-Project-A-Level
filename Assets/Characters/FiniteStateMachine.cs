using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Enums
public enum EnemyStates
{
    Active,
    Idle,
    Tracking,
    Attacking,
    Inactive
}

public enum Command
{
    Spawned,
    TargetNotInRange,
    TargetInRange,
    TargetInAttackingRange,
    TargetNotInAttackingRange,
    NoHealth,
}
#endregion

public class FiniteStateMachine
{
    class StateTransition
    {
        EnemyStates CurrentState;
        Command Command;

        public StateTransition(EnemyStates currentState, Command command)
        {
            CurrentState = currentState;
            Command = command;
        }
    }

    Dictionary<StateTransition, EnemyStates> transitions;
    public EnemyStates currentState { get; private set; }
    public EnemyStates previousState { get; private set; }

    public FiniteStateMachine()
    {
        currentState = EnemyStates.Inactive;
        transitions = new Dictionary<StateTransition, EnemyStates>
        {
            { new StateTransition(EnemyStates.Active, Command.Spawned), EnemyStates.Idle },
            { new StateTransition(EnemyStates.Idle, Command.TargetNotInRange), EnemyStates.Idle },
            { new StateTransition(EnemyStates.Idle, Command.TargetInRange), EnemyStates.Tracking },
            { new StateTransition(EnemyStates.Idle, Command.NoHealth), EnemyStates.Inactive },
            { new StateTransition(EnemyStates.Tracking, Command.TargetNotInRange), EnemyStates.Idle },
            { new StateTransition(EnemyStates.Tracking, Command.TargetInAttackingRange), EnemyStates.Attacking },
            { new StateTransition(EnemyStates.Tracking, Command.NoHealth), EnemyStates.Inactive },
            { new StateTransition(EnemyStates.Attacking, Command.TargetNotInAttackingRange), EnemyStates.Tracking },
            { new StateTransition(EnemyStates.Attacking, Command.NoHealth), EnemyStates.Inactive }
        };
    }

    public EnemyStates GetNext(Command command)
    {
        StateTransition transition = new StateTransition(currentState, command);
        EnemyStates nextState;
        if (!transitions.TryGetValue(transition, out nextState))
            throw new System.Exception("Invalid transition: " + currentState + " -> " + nextState);
        Debug.Log("Next state " + nextState);
        return nextState;
    }

    public EnemyStates MoveStates(Command command)
    {
        previousState = currentState;
        currentState = GetNext(command);
        Debug.Log("Moved states to: " + currentState);
        return currentState;
    }
}
