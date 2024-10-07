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

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        Move();
        Look();
    }
    private void Move()
    {

        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
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



