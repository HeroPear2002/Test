using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public IState CurrentState { get; private set; }
    public IdleState idleState;
    public JumpState jumpState;
    public MoveState moveState;
    public skill1 skill1;
    public FallState fallState;
    public JumpMiddle jumpMiddle;
    public DashState dashState;

    public StateMachine(PlayerController playerController)
    {
        idleState =  new IdleState(playerController);
        jumpState = new JumpState(playerController);
        moveState = new MoveState(playerController);
        skill1 = new skill1(playerController);
        fallState = new FallState(playerController);
        jumpMiddle = new JumpMiddle(playerController);
        dashState = new DashState(playerController);
    }

    public void Initialize(IState startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }
    public void TransitionTo(IState nextState)
    {
        Debug.Log(2);
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();
    }
    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }
}
