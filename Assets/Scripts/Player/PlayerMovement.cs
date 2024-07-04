using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CharacterController characterController;
    [SerializeField] Animator playerAnimator;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] PlayerAim playerAim;


    [Header("Parameters")]
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float runSpeed = 6f;
    [SerializeField] float turnSpeed = 6f;
    float speed;

    float verticalVelocity;
    float gravityScale = 9.81f;

    Vector3 moveDirection;

    public Vector2 moveInput { get;private set; }   

    bool isRunning;

    private void AssignInputEvents()
    {
        playerInput.PlayerActionsControls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        playerInput.PlayerActionsControls.Character.Movement.canceled += context => moveInput = Vector2.zero;


        playerInput.PlayerActionsControls.Character.Run.performed += context =>
        {
            if (moveDirection.magnitude > 0)
                speed = runSpeed; isRunning = true;
        };
        playerInput.PlayerActionsControls.Character.Run.canceled += context => { speed = walkSpeed; isRunning = false; };
    }

    private void Start()
    {
        AssignInputEvents();
        speed = walkSpeed;
    }

    private void Update()
    {
        ApplyMovement();
        ApplyRotation();
        AnimatorControllers();
    }

    private void AnimatorControllers()
    {
        float xVelocity = Vector3.Dot(moveDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(moveDirection.normalized, transform.forward);

        playerAnimator.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
        playerAnimator.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);



        playerAnimator.SetBool("IsRunning", isRunning);
    }
    private void ApplyRotation()
    {
        Vector3 lookDirection = playerAim.GetMouseHitInfo().point - transform.position;
        lookDirection.y = 0;
        lookDirection.Normalize();

        if(lookDirection == Vector3.zero)
            return;

        Quaternion desiredRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, turnSpeed * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGravity();

        if (moveDirection.magnitude > 0)
        {
            characterController.Move(moveDirection * Time.deltaTime * speed);
        }
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            verticalVelocity -= gravityScale * Time.deltaTime;
            moveDirection.y = verticalVelocity;
        }
        else
            verticalVelocity = -.5f;
    }
}
