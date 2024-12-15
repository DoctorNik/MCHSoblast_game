using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class FootstepsManager : MonoBehaviour
{
    [SerializeField] private State state;

    [Header("Тип поверхности")]
    public AudioClip[] snowFootsteps;   
    public AudioClip[] woodFootsteps;
    public AudioClip[] asphaltFootsteps;
    public AudioClip[] grassFootsteps;

    [Header("Частота")]
    public float walkInterval = 0.5f;   
    public float runInterval = 0.3f;
    public float stepInterval;

    private AudioSource audioSource;
    private float stepTimer = 0f;
    public string currentSurface = "snow";

    public bool IsWalk;
    public bool IsRun;

    [SerializeField] public PrintFootsteps _print;
    private AudioClip lastStepped;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (state != null)
        {
            state.OnStateChanged += StateChanged;
        }
    }

    void Update()
    {
        if (IsWalk || IsRun)
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                PlayFootstepSound();
                stepTimer = 0f;
            }
        }
    }

    void PlayFootstepSound()
    {
        AudioClip[] currentFootsteps = GetFootstepsArray();
        AudioClip clip;
        if (currentFootsteps.Length > 0)
        {
            do
            {
                clip = currentFootsteps[Random.Range(0, currentFootsteps.Length)];
            } while (clip == lastStepped); 

            lastStepped = clip; 

            audioSource.PlayOneShot(clip);
            _print.CreateFootstep();
        }
    }

    AudioClip[] GetFootstepsArray()
    {
        switch (currentSurface)
        {
            case "Snow":
                return snowFootsteps;
            case "Wood":
                return woodFootsteps;
            case "Asphalt":  
                return asphaltFootsteps;
            case "Grass":
                return grassFootsteps;
            default:
                return new AudioClip[0];
        }
    }

    public void SetSurface(string surfaceType)
    {
        currentSurface = surfaceType;
        if (currentSurface == "Snow")
        {
            _print.NotSnow = false;
            _print.GetSnow();
        }
        else
        {
            _print.NotSnow = true;
        }
    }

    private void StateChanged(State.PlayerState newState)
    {
        IsWalk = false;
        IsRun = false;

        switch (newState)
        {
            case State.PlayerState.Walk:
                IsWalk = true;
                stepInterval = walkInterval;
                break;

            case State.PlayerState.Run:
                IsRun = true;
                stepInterval = runInterval;
                break;

            default:
                stepInterval = walkInterval; 
                break;
        }
    }
}
