using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public enum CameraState
    {
        Calm,
        Walk,
        Run
    }

    [SerializeField] public PlayerMovement player;

    public float amplitude = 0.1f; 
    public float calmFrequency = 2.0f;
    public float walkFrequency = 3.0f;
    public float runFrequency = 5.0f;

    private Vector3 initialLocalPosition;
    public CameraState currentState = CameraState.Calm;

    private float targetFrequency;
    public float smoothTime = 0.1f;
    private float frequencyVelocity;

    void Awake()
    {
        player.MoveOn += ChangeState;
        player.RunOn += ChangeState;
        initialLocalPosition = transform.localPosition;

        targetFrequency = calmFrequency; 
    }

    private void ChangeState(int n)
    {
        switch (n)
        {
            case 1:
                currentState = CameraState.Calm;
                targetFrequency = calmFrequency;
                break;
            case 2:
                currentState = CameraState.Walk;
                targetFrequency = walkFrequency;
                break;
            case 3:
                currentState = CameraState.Run;
                targetFrequency = runFrequency;
                break;
            default:
                Debug.LogWarning("Неизвестное состояние: " + n);
                break;
        }
    }

    void FixedUpdate()
    {
        UpdateShake();
    }

    void UpdateShake()
    {
        /*float currentFrequency = Mathf.SmoothDamp(targetFrequency, targetFrequency, ref frequencyVelocity, smoothTime);
        float y = initialHeight + Mathf.Sin(Time.time * currentFrequency) * amplitude; 
        transform.position = new Vector3(transform.position.x, y, transform.position.z);*/

        float currentFrequency = Mathf.SmoothDamp(targetFrequency, targetFrequency, ref frequencyVelocity, smoothTime);
        float yOffset = Mathf.Sin(Time.time * currentFrequency) * amplitude;
        transform.localPosition = initialLocalPosition + new Vector3(0, yOffset, 0);
    }
}
