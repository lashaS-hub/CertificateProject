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
    [SerializeField] private float attackSpeed;


    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;



    [Space]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [Space]
    [SerializeField] private float jumpHeight;
    [Header("Camera")]
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float keyboardRotationSensitivity;
    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Slider healthSlider;


    #region References

    private PaladinInputSystem playerInput;
    private CharacterController characterController;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private Camera _camera;

    #endregion

    #region Variables

    private Vector2 InputMoveVec;
    private Vector2 ModifiedMoveVec;
    private Vector2 cameraRotateVec;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private int health;
    private int score;
    private float prevShootTime = 0;
    private bool isDied = false;
    private bool shiftButtonHeld = false;
    private bool cameraShouldRotate = false;


    private const float RUN_ANIMATION_MODIFIER = 2;

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


    }

    void Start()
    {
        _camera = Camera.main;
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
        InputMoveVec = context.ReadValue<Vector2>();

    }

    private void MoveCanceled(CallbackContext context)
    {
        InputMoveVec = context.ReadValue<Vector2>();

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
        if (isDied) return;
        var curTime = Time.time;
        if (curTime - prevShootTime > attackSpeed)
        {
            Debug.Log(++count);
            weapon.AttackStarted();
            Attack();
            prevShootTime = curTime;
        }
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // isGrounded = false;
        if (hit.transform.tag == "Ground")
        {
            // isGrounded = true;
            transform.SetParent(hit.transform);
        }
    }


    void Update()
    {

        Debug.Log(InputMoveVec);
        if (isDied) return;
        Move();
        CameraRotateWithMouse();
    }

    private void CameraRotateWithMouse()
    {
        if (!cameraShouldRotate) return;
        transform.Rotate(Vector3.up * cameraRotateVec.x * mouseSensitivity * Time.deltaTime);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Gizmos.DrawSphere(transform.position, groundCheckDistance);
        Gizmos.DrawSphere(transform.position, groundCheckDistance);
    }

    float animationVelocity_X;
    float animationVelocity_Z;

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);


        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        moveDirection = new Vector3(InputMoveVec.x, 0, InputMoveVec.y);
        moveDirection = transform.TransformDirection(moveDirection);

        if (isGrounded)
        {
            _animator.SetBool("IsJumping", false);

            moveSpeed = !shiftButtonHeld ? walkSpeed : runSpeed;

            animationVelocity_X = !shiftButtonHeld ? InputMoveVec.x : InputMoveVec.x * RUN_ANIMATION_MODIFIER;
            animationVelocity_Z = !shiftButtonHeld ? InputMoveVec.y : InputMoveVec.y * RUN_ANIMATION_MODIFIER;
            _animator.SetFloat("VelocityX", animationVelocity_X, .05f, Time.deltaTime);
            _animator.SetFloat("VelocityZ", animationVelocity_Z, .05f, Time.deltaTime);

            moveDirection *= moveSpeed;

            velocity.x = moveDirection.x;
            velocity.z = moveDirection.z;
        }

        // characterController.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }


    private void Idle()
    {
        _animator.SetFloat("Speed", .5f, .05f, Time.deltaTime);
    }

    private void WalkForward()
    {
        _animator.SetFloat("Speed", .75f, .05f, Time.deltaTime);
        moveSpeed = walkSpeed;
    }

    private void WalkBackward()
    {
        _animator.SetFloat("Speed", .25f, .05f, Time.deltaTime);
        moveSpeed = walkSpeed;
    }

    private void WalkRight()
    {

    }

    private void WalkLeft()
    {

    }

    private void RunForward()
    {
        // _animator.SetFloat("Speed", 1f, .05f, Time.deltaTime);
        _animator.SetFloat("Speed", 1f);
        moveSpeed = runSpeed;
    }

    private void RunBackward()
    {
        // _animator.SetFloat("Speed", 0f, .05f, Time.deltaTime);
        _animator.SetFloat("Speed", 0f);
        moveSpeed = runSpeed;
    }

    private void RunRight()
    {

    }

    private void RunLeft()
    {

    }


    private void Jump()
    {
        moveSpeed = walkSpeed;
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        _animator.SetBool("IsJumping", true);
        StartCoroutine(JumpAnimationAgain());
        // _animator.SetTrigger("Jump");
    }

    private IEnumerator JumpAnimationAgain()
    {
        yield return new WaitForSeconds(.1f);
        moveSpeed = walkSpeed;
        _animator.SetBool("IsJumping", true);
    }

    public void RestrictMovementWithFakeDeath()
    {
        _animator.SetFloat("Speed", .5f);
        isDied = true;
    }

    private void Die()
    {
        isDied = true;
        _animator.SetTrigger("Death");
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
        healthText.text = health.ToString();
    }

    private void AddScoreUI()
    {
        scoreText.text = score.ToString();
    }

    public Vector3 GetPlayerCenter()
    {
        return playerCenter.position;
    }

    public void DetachCamera()
    {
        _camera.transform.SetParent(null);
    }
}
