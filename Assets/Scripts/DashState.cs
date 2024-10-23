using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : IState
{
    PlayerController playerController;

    public DashState(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void Enter()
    {
        // code that runs when we first enter the state
        Debug.Log("idle state enter");
        playerController.anim.Play("Player_Dash");

    }
    public void Update()
    {
        
    }
    public void Exit()
    {
        // code that runs when we exit the state
        Debug.Log("idle state exit");
    }
}
