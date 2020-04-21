using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("PlayerSounds")]
    public AudioClip playerWalkingSound;
    public AudioClip playerRunningSound;
    public AudioClip playerHurtSound;
    public AudioClip playerDeadSound;
    public AudioClip playerJumpSound;
    public AudioClip playerCrouchSound;
    public AudioClip playerMeeleAttackSound;

    [Header("Player Actions")]
    public float walkSpeed;
    public float runSpeed;
    public float jumpForce;
    public float attackTime;
    public bool isFacingRight = true;
    private float movement;
    public LayerMask Ground;

    [Header("Player States")]
    private bool isGrounded;
    public enum AnimState
    {
        Idle, Walking, Running, Jumping, Crouched, Attacking, Hurt, Dead
    }
    public AnimState state;

    [Header("Unity Essentails")]
    private Rigidbody2D rb2d;
    private Animator animator;
    private float localScaleX;
    public Transform groundCheck;
    private Coroutine JumpCo, AttackCo;
    private static PlayerController Instance;
    public static PlayerController instance { get { return Instance; } }
    [Header("GamePlay")]
    [HideInInspector] public bool gamePaused;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        isGrounded = true;
        SetState("isIdle", true, AnimState.Idle);

    }
    void Update()
    {

        if (!gamePaused && !PlayerStats.instance.isDead)
        {
            movement = Input.GetAxis("Horizontal");

            if (shouldFlipCharacter())
                flip();

            if (state != AnimState.Jumping
            && state != AnimState.Attacking)
            {
                if (state != AnimState.Crouched)
                {
                    RunningAnim();

                    WalkingAnim();
                }

                JumpingAnim();

                CrouchingAnim();
            }
        }
    }
    public void Dead()
    {
        SetState("isDead", true, AnimState.Dead);
        SoundManager.instace.PlayAudio(playerDeadSound);
    }

    public void Hurt()
    {
        SetTrigger("Hurt", AnimState.Hurt);
        SoundManager.instace.PlayAudio(playerHurtSound);
    }

    private void CrouchingAnim()
    {
        if (Input.GetKeyDown(KeyCode.C) && state != AnimState.Crouched)
        {
            SetState("isIdle", false, state);
            SetState("isCrouched", true, AnimState.Crouched);
            SoundManager.instace.PlayAudio(playerCrouchSound);
        }
        else if (Input.GetKeyDown(KeyCode.C) && state == AnimState.Crouched)
        {
            ResetAnimations();
            SetState("isIdle", true, AnimState.Idle);
        }
    }

    private void JumpingAnim()
    {
        if (Input.GetKeyDown(KeyCode.Space) && state != AnimState.Crouched)
        {
            if (isGrounded && JumpCo == null)
            {
                JumpCo = StartCoroutine(Jump());
            }
        }
    }

    private void WalkingAnim()
    {
        if (movement != 0 && state != AnimState.Running)
        {
            SetState("isIdle", false, state);
            SetState("isWalking", true, AnimState.Walking);
            SoundManager.instace.PlayAudio(playerWalkingSound);

        }
        else if (movement == 0)
        {
            SetState("isWalking", false, state);
            SetState("isIdle", true, AnimState.Idle);
        }
    }

    private bool shouldFlipCharacter()
    {
        return (isFacingRight && movement < 0) || (!isFacingRight && movement > 0);
    }

    private void RunningAnim()
    {
        if (movement != 0 && Input.GetKey(KeyCode.LeftShift))
        {
            if (state != AnimState.Crouched)
            {
                SetState("isIdle", false, state);
                SetState("isWalking", false, state);
                SetState("isRunning", true, AnimState.Running);
                SoundManager.instace.PlayAudio(playerRunningSound);
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || movement == 0)
        {
            SetState("isRunning", false, state);
            if (state != AnimState.Walking && movement != 0 && state != AnimState.Crouched)
                SetState("isWalking", true, AnimState.Walking);
        }

    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, Ground);
        if (!gamePaused && !PlayerStats.instance.isDead)
            WalkRunLogic();
    }

    private void WalkRunLogic()
    {
        if (state != AnimState.Crouched)
        {
            if (state == AnimState.Walking || state == AnimState.Jumping)
                rb2d.velocity = new Vector2(movement * walkSpeed, rb2d.velocity.y);
            else if (state == AnimState.Running || state == AnimState.Jumping)
                rb2d.velocity = new Vector2(movement * runSpeed, rb2d.velocity.y);
        }
    }


    IEnumerator Jump()
    {
        isGrounded = false;
        SetState("isIdle", false, state);
        SetTrigger("Jump", AnimState.Jumping);

        SoundManager.instace.PlayAudio(playerJumpSound, true);

        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        yield return new WaitUntil(() => isGrounded);
        isGrounded = true;
        SetState("isIdle", true, AnimState.Idle);
        StopJumpCoroutine();
    }
    void flip()
    {
        isFacingRight = !isFacingRight;
        localScaleX = transform.localScale.x;
        localScaleX *= -1;
        transform.localScale = new Vector2(localScaleX, transform.localScale.y);
    }
    public void ResetAnimations()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isCrouched", false);
        animator.SetBool("isDead", false);
        animator.SetBool("isIdle", false);
        animator.ResetTrigger("Jump");
        animator.ResetTrigger("Hurt");
        animator.ResetTrigger("Staff");
    }
    public void Attack()
    {
        if (AttackCo == null)
            AttackCo = StartCoroutine(AttackCoroutine());
        else return;
    }
    private IEnumerator AttackCoroutine()
    {
        SetState("isIdle", false, state);
        SetTrigger("Staff", AnimState.Attacking);
        SoundManager.instace.PlayAudio(playerMeeleAttackSound, true);


        yield return new WaitForSeconds(attackTime);
        SetState("isIdle", true, AnimState.Idle);
        StopAttack();
    }
    private void StopAttack()
    {
        if (AttackCo != null)
        {
            StopCoroutine(AttackCo);
            AttackCo = null;
        }
    }
    private void StopJumpCoroutine()
    {
        if (JumpCo != null)
        {
            StopCoroutine(JumpCo);
            JumpCo = null;
        }
    }

    private void SetTrigger(string triggerName, AnimState setState)
    {
        animator.SetTrigger(triggerName);
        state = setState;
    }
    private void SetState(string boolName, bool boolValue, AnimState setState)
    {
        animator.SetBool(boolName, boolValue);
        state = setState;
    }

}
