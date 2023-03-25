using System.Collections.Generic;
using UnityEngine;

#region Enums
public enum EnemyStates
{
    Active,
    Idle,
    Tracking,
    Attacking,
    Inactive,
}

public enum EnemyCommands
{
    Spawned,
    InRange,
    NotInRange,
    InAttackRange,
    NotInAttackRange,
    NoHealth,
}
#endregion

public class EnemyFSM : StateMachine<EnemyStates, EnemyCommands>
{
    #region Fields
    #region Object References
    public Active activeState
    { get; private set; }
    public Idle idleState
    { get; private set; }
    public Tracking trackingState
    { get; private set; }
    public Attacking attackingState
    { get; private set; }
    public Inactive inactiveState
    { get; private set; }
    #endregion
    #endregion

    #region States
    public class Active : State
    {
        public Active() : base(EnemyStates.Active, false) { }

        public override void Enter()
        {
            base.Enter();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }

        public override void UpdateFixed()
        {
            base.UpdateFixed();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    public class Idle : State
    {
        public Idle() : base(EnemyStates.Idle, false) { }

        public override void Enter()
        {
            base.Enter();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }

        public override void UpdateFixed()
        {
            base.UpdateFixed();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    public class Tracking : State
    {
        public Tracking() : base(EnemyStates.Tracking, false) { }

        public override void Enter()
        {
            base.Enter();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }

        public override void UpdateFixed()
        {
            base.UpdateFixed();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    public class Attacking : State
    {
        public Attacking() : base(EnemyStates.Attacking, false) { }

        public override void Enter()
        {
            base.Enter();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }

        public override void UpdateFixed()
        {
            base.UpdateFixed();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    public class Inactive : State
    {
        public Inactive() : base(EnemyStates.Inactive, true) { }

        public override void Enter()
        {
            base.Enter();
            // Give points to the player          
            // Destroy gameobject
        }
    }
    #endregion

    public EnemyFSM() : base()
    {
        MakeStates();
        this.currentState = activeState;
        MakeTransitionTable();
    }

    private void MakeTransitionTable()
    {
        this.transitionTable = new Dictionary<StateTransition, State>()
        {
            { new StateTransition(activeState, EnemyCommands.Spawned), idleState },
            { new StateTransition(idleState, EnemyCommands.NotInRange), idleState },
            { new StateTransition(idleState, EnemyCommands.InRange), trackingState },
            { new StateTransition(idleState, EnemyCommands.NoHealth), inactiveState },
            { new StateTransition(trackingState, EnemyCommands.NotInRange), idleState },
            { new StateTransition(trackingState, EnemyCommands.InAttackRange), attackingState },
            { new StateTransition(trackingState, EnemyCommands.NoHealth), inactiveState },
            { new StateTransition(attackingState, EnemyCommands.NotInAttackRange), trackingState },
            { new StateTransition(attackingState, EnemyCommands.NoHealth), inactiveState },
        };

        //foreach (var kvp in transitionTable)
        //{
        //    Debug.Log("CurrentState: " + kvp.Key.currentState.thisStateID + " Command: " + kvp.Key.command + " NextState: " + kvp.Value.thisStateID);
        //}      
    }

    private void MakeStates()
    {
        activeState = new Active();
        idleState = new Idle();
        trackingState = new Tracking();
        attackingState = new Attacking();
        inactiveState = new Inactive();
    }
}
