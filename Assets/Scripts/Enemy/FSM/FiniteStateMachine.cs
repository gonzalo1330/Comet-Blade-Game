using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    // keeps track of the current state
    // variable is public to get (any other class can get this variable)
    // this variable can only be set in this class
    public State currentState { get; private set; }

    public void Initialize(State startingState) {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(State newState) {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
    
}
