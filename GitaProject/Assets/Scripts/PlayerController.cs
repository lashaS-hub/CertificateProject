using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static UnityEngine.InputSystem.InputAction;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerWeaponController weapon;
    [Header("Variables")]
    [SerializeField] private Transform playerCenter;
    [SerializeField] private int maxHealth;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float attackSpeed;
    [Space]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [Space]
    [SerializeField] private float jumpHeight;
    [Header("Camera")]
    [SerializeField] private float mouseSensitivity;
    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Slider healthSlider;


    #region References

    private PaladinInputSystem playerInput;
    private CharacterController characterController;
    private Rigidbody _rigidbody;
    private Animator _animator;

    #endregion

    #region Variables

    private Vector2 moveVec;
    private Vector2 cameraRotateVec;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private int health;
    private int score;
    private float prevShootTime = 0;
    private bool isDied = false;
    private bool moveButtonHeld = false;
    private bool shiftButtonHeld = false;
    private bool cameraShouldRotate = false;
    private bool isAttacking = false;


    #endregion

    private void Awake()
    {
        playerInput = new PaladinInputSystem();
        characterController = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();

        playerInput.Player.Move.performed += MovePerformed;
        playerInput.Player.Move.canceled += MoveCanceled;

        playerInput.Player.Run.performed += RunPerformed;
        playerInput.Player.Run.canceled += RunCanceled;

        playerInput.Player.Jump.performed += JumpPerformed;

        playerInput.Player.Look.performed += LookPerformed;
        playerInput.Player.Look.canceled += LookCanceled;

        playerInput.Player.Fire.performed += AttackPerformed;
        playerInput.Player.Fire.canceled += AttackCanceled;


    }

    void Start()
    {
        health = maxHealth;

        UpdateHealth();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnEnable()
    {
        playerInput.Player.Enable();
    }

    void OnDisable()
    {
        playerInput.Player.Disable();
    }

    private void MovePerformed(CallbackContext context)
    {
        moveVec = context.ReadValue<Vector2>();
        moveButtonHeld = true;
    }

    private void MoveCanceled(CallbackContext context)
    {
        moveVec = context.ReadValue<Vector2>();
        moveButtonHeld = false;
    }

    private void RunPerformed(CallbackContext context)
    {
        shiftButtonHeld = true;
    }

    private void RunCanceled(CallbackContext context)
    {
        shiftButtonHeld = false;
    }

    private void JumpPerformed(CallbackContext context)
    {
        Jump();
    }

    private void LookPerformed(CallbackContext value)
    {
        cameraShouldRotate = true;
        cameraRotateVec = value.ReadValue<Vector2>();
    }

    private void LookCanceled(CallbackContext value)
    {
        cameraShouldRotate = false;
    }
    int count = 0;
    private void AttackPerformed(CallbackContext value)
    {
        var curTime = Time.time;
        if (curTime - prevShootTime > attackSpeed)
        {
            isAttacking = true;
            Debug.Log(++count);
            weapon.AttackStarted();
            Attack();
            prevShootTime = curTime;
        }
    }

    private void AttackCanceled(CallbackContext value)
    {
        weapon.AttackFinished();
        isAttacking = false;
    }

    void Update()
    {
        if (isDied) return;
        Move();
        CameraRotate();
    }

    private void CameraRotate()
    {
        if (!cameraShouldRotate) return;
        transform.Rotate(Vector3.up * cameraRotateVec.x * mouseSensitivity * Time.deltaTime);
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        moveDirection = new Vector3(0, 0, moveVec.y);
        moveDirection = transform.TransformDirection(moveDirection);

        if (isGrounded)
        {
            if (moveVec.y == -1 && !shiftButtonHeld)
            {
                WalkBackward();
            }
            else if (moveVec.y == -1 && shiftButtonHeld)
            {
                RunBackward();
            }
            else if (moveVec.y == 1 && !shiftButtonHeld)
            {
                Walk();
            }
            else if (moveVec.y == 1 && shiftButtonHeld)
            {
                Run();
            }
            else if (moveVec.y == 0)
            {
                Idle();
            }

            moveDirection *= moveSpeed;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void RunBackward()
    {
        _animator.SetFloat("Speed", 0f, .05f, Time.deltaTime);
        moveSpeed = runSpeed;
    }

    private void WalkBackward()
    {
        _animator.SetFloat("Speed", .25f, .05f, Time.deltaTime);
        moveSpeed = walkSpeed;
    }

    private void Walk()
    {
        _animator.SetFloat("Speed", .75f, .05f, Time.deltaTime);
        moveSpeed = walkSpeed;
    }

    private void Idle()
    {
        _animator.SetFloat("Speed", .5f, .05f, Time.deltaTime);
    }


    private void Run()
    {
        _animator.SetFloat("Speed", 1f, .05f, Time.deltaTime);
        moveSpeed = runSpeed;
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    private void Die()
    {
        isDied = true;
        _animator.SetTrigger("Death");
        Debug.Log("die");
    }

    private void Attack()
    {
        _animator.SetTrigger("Attack");
    }

    public void GetDamaged(int damage)
    {
        health -= damage;

        UpdateHealth();

        if (health <= 0)
        {
            Die();
        }
        else
        {

        }
    }

    public void HealUp()
    {
        health = maxHealth;
        UpdateHealth();
    }

    public void ScoreUp()
    {
        score++;
        AddScoreUI();
    }

    private void UpdateHealth()
    {
        healthSlider.value = health;
    }

    private void AddScoreUI()
    {
        scoreText.text = score.ToString();
    }

    public Vector3 GetPlayerCenter()
    {
        return playerCenter.position;
    }
}
