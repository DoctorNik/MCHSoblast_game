using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class State : MonoBehaviour
{
    public enum PlayerState
    {
        Calm,
        Walk,
        Run
    }
    [SerializeField] private PlayerMovement player;  
    public PlayerState currentState = PlayerState.Calm;

    public Action<PlayerState> OnStateChanged;

    public void ChangeState(PlayerState newState)
    {
        currentState = newState;
        OnStateChanged?.Invoke(currentState);
    }
}
