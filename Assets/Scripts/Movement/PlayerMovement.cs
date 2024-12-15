using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Camera CameraHolder;
    [SerializeField] float mouseSensivity, sprintSpeed, walkSpeed, smoothTime, jumpForce;

    bool grounded;
    float verticalLookRotation;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;
    public PlayerHands Hand;
    Rigidbody rb;

    [SerializeField]public RaycastAim raycast;

    [SerializeField] private State state;

    private bool stop;
    public bool STOP
    {
        get { return stop; }
        set
        {
            stop = value;
            if (stop)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    private bool moving;
    public Action<int> MoveOn;
    private bool running;
    public Action<int> RunOn;
    private void Awake()
    {
        raycast = FindObjectOfType<RaycastAim>();
        Hand = FindObjectOfType<PlayerHands>();
        raycast.MustStop += ChangeSTOP;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    private void ChangeSTOP(bool change)
    {
        STOP = change;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        if (STOP)
        {
            moveAmount = Vector3.zero;
            return;
        }

        Move();
        Look();

        bool isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && isMoving && !Hand.HeavyPicked;

        if (isMoving)
        {
            state.ChangeState(isRunning ? State.PlayerState.Run : State.PlayerState.Walk);
        }
        else
        {
            state.ChangeState(State.PlayerState.Calm);
        }
    }
    private void Move()
    {
        if (STOP)
        {
            moveAmount = Vector3.zero; 
            return; 
        }
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) && !Hand.HeavyPicked ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    void Look()
    {

        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensivity ;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        CameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);

    }
}



