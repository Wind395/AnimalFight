using UnityEngine;
using Fusion;
using System;

public class AnimalMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpHeight = 10f;
    private float gravity = -9.81f;
    private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float smoothTime;
    private float smoothAngle = 0.1f;

    Vector3 direction;

    //Rigidbody rb;
    public CharacterController characterController;
    Animator animator;
    //AnimalCamera animalCamera;
    Vector3 velocity;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        //animalCamera = GetComponent<AnimalCamera>();
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    void Update()
    {
        
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if (!GameManager.Instance.canMove) return;
        //if (!HasInputAuthority) return;
        //if (ChatBoxManager.Instance.isChatOpen) return;
        //Debug.Log("Can Move: " + GameManager.Instance.canMove);
        Movement();
        Jump();
        
    }

        /// <summary>
        /// Called on every frame, this function is responsible for updating the camera's rotation to match the player's rotation.
        /// </summary>
    public override void Render()
    {
        if (!GameManager.Instance.canMove) return;
        Rotation();

        if (Input.GetButtonDown("Fire1") && Object.HasInputAuthority)
        {
            //Debug.Log("Attack triggered by local player");
            RPC_TriggerAttack();
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void RPC_TriggerAttack()
    {
        AnimalsWeapon weaponScript = GetComponent<AnimalsWeapon>();
        if (weaponScript != null && weaponScript.currentWeapon != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        direction = new Vector3(horizontal, 0, vertical).normalized;

        if (velocity.y < 0)
        {
            velocity.y = -2;
        }

        velocity.y += gravity * Runner.DeltaTime;
        characterController.Move(velocity * Runner.DeltaTime);

        animator.SetFloat("Idle", direction.magnitude);
        animator.SetFloat("Run", direction.magnitude);

        characterController.Move(direction * speed * Runner.DeltaTime);
        //rb.linearVelocity = new Vector3(direction.x * speed, rb.linearVelocity.y, direction.z * speed);
    }

    void Jump()
    {
        //Debug.Log(isGrounded);
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void Rotation()
    {
        if (direction.magnitude >= 0.1f)
        {
            // Tạo Quaternion để xoay theo hướng di chuyển
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            // Giới hạn xoay quanh trục Y
            targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

            // Áp dụng xoay trực tiếp
            transform.rotation = targetRotation;
        }
    }
}
