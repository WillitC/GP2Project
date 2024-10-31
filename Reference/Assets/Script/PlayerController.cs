using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float rotationY = 0.0f;
    float rotationX = 0.0f; // For vertical camera movement
    float verticalInput = 0.0f;
    float horizontalInput = 0.0f;
    public float speedValue = 50.0f;
    public float rotationSpeed = 15.0f;
    public float cameraSensitivity = 100.0f;
    public Transform cameraTransform; // Drag your camera object here in the Inspector
    public float maxLookAngle = 85f; // Limits camera vertical movement

    public GameObject[] tireSFX;

    Vector3 playerVelocity;
    Vector3 move;

    public float walkSpeed = 5;
    public float runSpeed = 8;

    public float jumpHeight = 2;
    public int maxJumpCount = 1;
    private int currentJumpCount = 1;

    public float gravity = -9.81f;

    private float lastPressed;

    public bool isGrounded;
    public bool isRunning;
    public bool isJumping;

    private CharacterController controller;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Confines the cursor to the game window
        Cursor.visible = false;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        // Only process input and movement if root motion is not applied
        if (!animator.applyRootMotion)
        {
            ProcessMovement();
        }

        // Always process gravity regardless of root motion
        ProcessGravity();

        // Movement input
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        // Camera rotation based on mouse input
        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;

        rotationY += mouseX;
        rotationX -= mouseY; // Invert to make camera look up/down correctly
        rotationX = Mathf.Clamp(rotationX, -maxLookAngle, maxLookAngle); // Prevent the camera from looking too far up or down

        // Rotate player horizontally
        transform.rotation = Quaternion.Euler(0.0f, rotationY, 0.0f);

        // Apply vertical rotation to the camera (only rotate the camera, not the player)
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);

    }

    void ProcessMovement()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        var forward = cameraTransform.forward;
        var right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        move = forward * verticalAxis + right * horizontalAxis;
        // Turns the player toward where they want to go
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
        // Check input to know if running is getting pressed
        isRunning = Input.GetKey(KeyCode.LeftShift);
        if (Input.GetButtonDown("Jump") && isGrounded) // Code to jump
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            lastPressed = Time.time;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            float pressTime = (Time.time - lastPressed);
            if (pressTime < 0.2f)
            {
                StartCoroutine(DashCoroutine());
                int chosenSFX = Random.Range(0, 1);
                GameObject tire = tireSFX[chosenSFX];
                GameObject sfx = Instantiate(tire, transform.position, transform.rotation);
                sfx.transform.parent = transform;
                Destroy(sfx, 1);
            }
        }

        controller.Move(move * Time.deltaTime * ((isRunning) ? runSpeed : walkSpeed));
    }

    private IEnumerator DashCoroutine()
    {
        float startTime = Time.time;
        while (Time.time < startTime + 0.2)
        {
            controller.Move(move * 45 * Time.deltaTime);
            yield return null;
        }
    }

    public void ProcessGravity()
    {
        // Since there is no physics applied on character controller, apply gravity manually
        if (isGrounded)
        {
            if (playerVelocity.y < 0.0f) // Ensure player stays grounded when on the ground
            {
                playerVelocity.y = -1.0f;
            }
        }
        else // If not grounded
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }

        controller.Move(playerVelocity * Time.deltaTime);


    }

    public int getCurrentJumpCount() { return currentJumpCount; }

    public void jumpBuffEnabled()
    {
        maxJumpCount = 2;
    }

    public void jumpBuffDisabled()
    {
        maxJumpCount = 1;
    }

    private void OnAnimatorMove()
    {
        if (animator.applyRootMotion)
        {
            Vector3 velocity = animator.deltaPosition;
            //velocity.y = playerVelocity.y * Time.deltaTime;
           /* if (isVaulting)
            { velocity *= (wallBounds.z); }*/
            controller.Move(velocity);
        }

    }

    public float GetMoveSpeed()
    {
        if (isRunning && (move != Vector3.zero))// Left shift
        {
            return runSpeed;
        }
        else if (move != Vector3.zero)
        {
            return walkSpeed;
        }
        else
        {
            return 0f;
        }
    }

    public float GetVerticalVelocity()
    {
        return playerVelocity.y;
    }

    public bool isActing()
    {
        if (isJumping)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
