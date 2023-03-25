using System;
using System.Collections.Generic;
using UnityEngine;

// Generic type parameters where the constraint is making sure they are enums
public class StateMachine <T1, T2> where T1 : Enum where T2 : Enum
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
    }

    // A class that defines all states of the state machine, set to a generic type of an enum
    public class State : MonoBehaviour
    {
        #region Fields
        #region Getters and Setters
        public T1 thisStateID
        { get; set; }

        // Whether or not the state is the accepting state or not
        public bool acceptingState
        { get; protected set; }
        #endregion
        #endregion

        protected State(T1 thisStateID, bool acceptingState)
        {
            this.thisStateID = thisStateID;
            this.acceptingState = acceptingState;
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
    { get; set; }
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
        Debug.Log(currentState.thisStateID);
        StateTransition transition = new StateTransition(currentState, command);
        Debug.Log(transition.currentState.thisStateID);
        Debug.Log(transition.command);
        
        // Looks up the transition in the table (key) and sees whether or not the transition is valid and returns the next state if it is
        if (!transitionTable.TryGetValue(transition, out State nextState))
        {
            throw new Exception("Invalid Transition " + currentState.thisStateID + " by " + command + " -> " + nextState); // STATES ARE NULL, NEEDS FIXING
        }
        
        Debug.Log(nextState.thisStateID);
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
        currentState.Enter();
        Debug.Log("Now in state: " + currentState.thisStateID.ToString());
    }
}
