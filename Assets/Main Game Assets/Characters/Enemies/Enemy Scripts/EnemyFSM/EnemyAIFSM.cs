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

public class EnemyAIFSM : StateMachine<EnemyStates, EnemyCommands, EnemyAIFSM>
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

    #region Getters and Setters
    public float distanceFromTarget
    { get; set; }
    public float maxTrackDistance
    { get; set; }
    public float attackDistance
    { get; private set; }
    #endregion
    #endregion

    #region States
    public class Active : State
    {
        public Active(EnemyAIFSM enemyAIFSM) : base(EnemyStates.Active, false, enemyAIFSM) { }

        public override void Enter()
        {
            belongsTo.maxTrackDistance = 15f;
            belongsTo.attackDistance = 2.5f;
        }

        public override void Exit() { base.Exit(); }
    }

    public class Idle : State
    {
        public Idle(EnemyAIFSM enemyAIFSM) : base(EnemyStates.Idle, false, enemyAIFSM) { }

        public override void Enter() { base.Enter(); }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }

        public override void UpdateFixed()
        {
            // Only moves states if it is less than or equal to the max tracking distance
            if (belongsTo.distanceFromTarget <= belongsTo.maxTrackDistance)
            {
                belongsTo.MoveStates(EnemyCommands.InRange);
            }
        }

        public override void Exit() { base.Exit(); }
    }

    public class Tracking : State
    {
        public Tracking(EnemyAIFSM enemyAIFSM) : base(EnemyStates.Tracking, false, enemyAIFSM) { }

        public override void Enter() { base.Enter(); }

        public override void UpdateFixed()
        {
            if (belongsTo.distanceFromTarget >= belongsTo.maxTrackDistance)
            {
                belongsTo.MoveStates(EnemyCommands.NotInRange);
            }
            else if (belongsTo.distanceFromTarget <= belongsTo.attackDistance)
            {
                belongsTo.MoveStates(EnemyCommands.InAttackRange);
            }
        }

        public override void Exit() { base.Exit(); }
    }

    public class Attacking : State
    {
        public Attacking(EnemyAIFSM enemyAIFSM) : base(EnemyStates.Attacking, false, enemyAIFSM) { }

        public override void Enter() { base.Enter(); }

        public override void UpdateFixed()
        {
            if (belongsTo.distanceFromTarget > belongsTo.attackDistance)
            {
                belongsTo.MoveStates(EnemyCommands.NotInAttackRange);
            }
        }

        public override void Exit() { base.Exit(); }
    }

    public class Inactive : State
    {
        public Inactive(EnemyAIFSM enemyAIFSM) : base(EnemyStates.Inactive, true, enemyAIFSM) { }

        public override void Enter()
        {
            
        }
    }
    #endregion

    public EnemyAIFSM() : base()
    {
        MakeTransitionTable();
        this.currentState = activeState;
        currentState.Enter();
    }

    private void MakeTransitionTable()
    {
        activeState = new Active(this);
        idleState = new Idle(this);
        trackingState = new Tracking(this);
        attackingState = new Attacking(this);
        inactiveState = new Inactive(this);
        
        this.transitionTable = new Dictionary<StateTransition, State>
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
        //    Debug.Log("CurrentState: " + kvp.Key.currentState + " Command: " + kvp.Key.command + " NextState: " + kvp.Value);
        //    //if (kvp.Key.currentState == null)
        //    //{
        //    //    Debug.Log("N");
        //    //}
        //}
    }
}
