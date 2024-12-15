using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static State;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private State state;

    public float amplitude = 0.1f; 
    public float calmFrequency = 2.0f;
    public float walkFrequency = 3.0f;
    public float runFrequency = 5.0f;

    private Vector3 initialLocalPosition;

    private float targetFrequency;
    public float smoothTime = 0.1f;
    private float frequencyVelocity;

    void Awake()
    {
        initialLocalPosition = transform.localPosition;
        state.OnStateChanged += UpdateFrequencyBasedOnState;
        targetFrequency = calmFrequency; 
    }
    void FixedUpdate()
    {
        UpdateShake();
    }

    private void UpdateFrequencyBasedOnState(State.PlayerState newState)
    {
        switch (newState)
        {
            case State.PlayerState.Calm:
                targetFrequency = calmFrequency;
                break;
            case State.PlayerState.Walk:
                targetFrequency = walkFrequency;
                break;
            case State.PlayerState.Run:
                targetFrequency = runFrequency;
                break;
        }
    }

    void UpdateShake()
    {
        float currentFrequency = Mathf.SmoothDamp(targetFrequency, targetFrequency, ref frequencyVelocity, smoothTime);
        float yOffset = Mathf.Sin(Time.time * currentFrequency) * amplitude;
        transform.localPosition = initialLocalPosition + new Vector3(0, yOffset, 0);
    }
}
