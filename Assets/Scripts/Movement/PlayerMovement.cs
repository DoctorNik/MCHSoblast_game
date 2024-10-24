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
        }
        if (!STOP)
        {
            Move();
            Look();
        }
        bool isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;

        if (!moving && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
        {
            moving = true;
            MoveOn?.Invoke(2);
        }
        else if (moving && (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0))
        {
            moving = false;
            MoveOn?.Invoke(1);
        }
        if (!running && Input.GetKey(KeyCode.LeftShift) && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && !Hand.HeavyPicked)
        {
            running = true;
            RunOn?.Invoke(3); 
        }
        else if (running && !Input.GetKey(KeyCode.LeftShift))
        {
            running = false;
            MoveOn?.Invoke(2);
        }
        if (running && (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0))
        {
            running = false;
            RunOn?.Invoke(1);
        }
        if (Hand.HeavyPicked)
        {
            if (isMoving)
            {
                RunOn?.Invoke(2);
            }
            running = false; 
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



