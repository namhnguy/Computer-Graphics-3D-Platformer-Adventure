using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] CinemachineFreeLook vcam;
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpGracePeriod;
    [SerializeField] List<AudioClip> audioClips;

    [SerializeField] float runSpeed;
    [SerializeField] float gravityScale;

    AudioSource audioSource;
    CharacterController controller;
    Animator animator;
    float ySpeed;
    float currentStepOffset;
    float? lastGroundTime;
    float? jumpPressedTime;
    float horizontal;
    float vertical;
    Vector3 direction;
    float magnitude;
    bool isBounce = false;
    float bounceForce;
    float currentSpeed;
    Vector3 startingPosition;
    bool isRunning;
    float rad = 20f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentStepOffset = controller.stepOffset;
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        GetCameraInput();
        GetPlayerInput();
        ProcessDirection();
        CheckGrounded();
        CheckJumpButton();
        CharacterMove();
    }

    void GetCameraInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Input.GetMouseButton(1))
            {
                vcam.m_YAxis.m_MaxSpeed = 10;
                vcam.m_XAxis.m_MaxSpeed = 500;
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            vcam.m_YAxis.m_MaxSpeed = 0;
            vcam.m_XAxis.m_MaxSpeed = 0;
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                rad -= Input.mouseScrollDelta.y;
                if (rad < 10)
                    rad = 10f;
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                rad -= Input.mouseScrollDelta.y;
                if (rad > 30)
                    rad = 30f;
            }
            for (int i = 0; i < vcam.m_Orbits.Length; i++)
            {
                vcam.m_Orbits[i].m_Radius = rad;
            }
        }
    }

    void GetPlayerInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    void ProcessDirection()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            currentSpeed = runSpeed;
        }
        else
        {
            isRunning = false;
            currentSpeed = movementSpeed;
        }
        direction = new Vector3(horizontal, 0, vertical);
        magnitude = Mathf.Clamp01(direction.magnitude) * currentSpeed;
        direction = Quaternion.AngleAxis(cam.rotation.eulerAngles.y, Vector3.up) * direction;
        direction.Normalize();
    }

    void CheckGrounded()
    {
        if (controller.isGrounded)
        {
            lastGroundTime = Time.time;
        }
        else
        {
            animator.SetBool("isFalling", true);
        }
    }

    void CheckJumpButton()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressedTime = Time.time;
        }
    }

    void CharacterMove()
    {
        ySpeed += (Physics.gravity.y * Time.deltaTime * gravityScale);     //addded a gravity scale to help adjust 'floatiness'

        if (Time.time - lastGroundTime <= jumpGracePeriod)
        {
            controller.stepOffset = currentStepOffset;
            ySpeed = -2f;
            animator.SetBool("isFalling", false);
            if (Time.time - jumpPressedTime <= jumpGracePeriod)
                Jump();
        }
        else
            controller.stepOffset = 0;

        if (isBounce)
        {
            audioSource.PlayOneShot(audioClips[2]);
            ySpeed = bounceForce;
            isBounce = false;
            jumpPressedTime = null;
            lastGroundTime = null;
        }

        Vector3 velocity = direction * magnitude;
        velocity = AdjustVelocityToSlope(velocity);
        velocity.y += ySpeed;

        controller.Move(velocity * Time.deltaTime);

        if (direction != Vector3.zero)
        {
            if (isRunning)
            {
                animator.SetBool("isMoving", true);
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isMoving", true);
                animator.SetBool("isRunning", false);
            }
            Quaternion rotateTo = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, rotationSpeed * Time.deltaTime);
        }
        else
            animator.SetBool("isMoving", false);
    }

    void Jump()
    {
        animator.SetTrigger("Jump");
        audioSource.PlayOneShot(audioClips[0]);
        ySpeed = jumpForce;
        jumpPressedTime = null;
        lastGroundTime = null;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Bounce") && hit.moveDirection == new Vector3(0f, -1f, 0f))
        {
            isBounce = true;
            bounceForce = hit.collider.GetComponent<BouncePlatform>().BounceForce;
        }
    }

    Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.2f))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;

            if (adjustedVelocity.y < 0)
            {
                return adjustedVelocity;
            }
        }
        return velocity;
    }

    internal void ResetToStart()
    {
        audioSource.PlayOneShot(audioClips[1]);
        vcam.gameObject.SetActive(false);
        vcam.PreviousStateIsValid = false;
        transform.position = startingPosition;
        vcam.gameObject.SetActive(true);
    }
}