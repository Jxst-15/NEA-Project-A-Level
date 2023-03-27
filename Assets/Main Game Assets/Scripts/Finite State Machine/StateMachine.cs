using System;
using System.Collections.Generic;
using UnityEngine;

// Generic type parameters where the constraint is making sure they are enums (T1 and T2)
// T3 is used for determining which fsm the state belongs to, it is a class
public class StateMachine <T1, T2, T3> where T1 : Enum where T2 : Enum where T3 : class
{
    #region Nested Classes
    // A class that defines the state transition
    public class StateTransition
    {
        #region Getters and Setters
        public State currentState
        { get; set; }

        public T2 command
        { get; set; }
        #endregion

        public StateTransition(State currentState, T2 command)
        {
            this.currentState = currentState;
            this.command = command;
        }

        // Need to figure out what this does
        public override bool Equals(object obj)
        {
            StateTransition other = obj as StateTransition;
            return other != null && this.currentState.Equals(other.currentState) && this.command.Equals(other.command);
        }

        public override int GetHashCode()
        {
            return 17 + 31 * this.currentState.GetHashCode() + 31 * this.command.GetHashCode();
        }
    }

    // A class that defines all states of the state machine, set to a generic type of an enum
    public class State
    {
        #region Fields
        #region Getters and Setters
        public T1 thisStateID
        { get; set; }

        // Whether or not the state is the accepting state or not
        public bool acceptingState
        { get; protected set; }

        public T3 belongsTo
        { get; protected set; }
        #endregion
        #endregion

        protected State(T1 thisStateID, bool acceptingState, T3 belongsTo)
        {
            this.thisStateID = thisStateID;
            this.acceptingState = acceptingState;
            this.belongsTo = belongsTo;
        }

        // Methods used to handle what happens in a transition
        #region State Methods
        public virtual void Enter() { }
        public virtual void UpdateLogic() { }
        public virtual void UpdateFixed() { }
        public virtual void Exit() { }
        #endregion
    }
    #endregion

    #region Fields
    // The transition table used to store all transitions of the state machine
    protected Dictionary<StateTransition, State> transitionTable;
    
    #region Getters and Setters
    public State currentState
    { get; protected set; }
    public State previousState
    { get; protected set; }
    #endregion
    #endregion

    // This constructor checks if the given generic parameters are enums
    protected StateMachine()
    {
        if(typeof (T1).IsEnum != true || typeof (T2).IsEnum != true)
        {
            throw new Exception("Parameters are not an enum.");
        }
    }

    // Makes sure the next state is actually in the transition table
    private State CheckIfTransitionValid(T2 command)
    {
        StateTransition transition = new StateTransition(currentState, command);
        
        // Looks up the transition in the table (key) and sees whether or not the transition is valid and returns the next state if it is
        if (!transitionTable.TryGetValue(transition, out State nextState))
        {
            throw new Exception("Invalid Transition " + currentState + " by " + command + " -> " + nextState); // STATES ARE NULL, NEEDS FIXING
        }
  
        // Output if it is
        return nextState;
    }

    // Transitions the states
    public virtual void MoveStates(T2 command)
    {
        // Exits the current state
        currentState.Exit();
        previousState = currentState;
        currentState = CheckIfTransitionValid(command);
        Debug.Log("Now in state: " + currentState.thisStateID);
        currentState.Enter();
    }
}
