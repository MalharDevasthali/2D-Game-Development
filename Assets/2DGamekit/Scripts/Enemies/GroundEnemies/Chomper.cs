using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chomper : MonoBehaviour
{

    [Header("Chomper Health")]
    public float maxHealth;
    [HideInInspector] public float currenthealth;

    [Header("Chomper Movement")]
    public float walkSpeed;
    public float runSpeed;
    public bool isFacingRight = true;
    public States currentState;

    [Header("Unity Essentials")]
    private Animator animator;
    private Rigidbody2D rb2d;
    public Transform LeftBound, RightBound;
    private Coroutine AttackCo, HurtCo;
    private float localScaleX;

    [Header("Attack Parameters")]
    public float attackDistance;
    public float attackCoolDown;
    public bool isPlayerDetected = false;
    public float KnockBackForce, knockBackTime;

    private void Start()
    {

        currenthealth = maxHealth;
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        SetState("isWalking", true, States.walking);
    }

    public enum States
    {
        coolDown, running, walking, attacking, hurt, dead,
    }

    private void Update()
    {
        if (currentState == States.hurt) return;

        if (shouldFlip())
            Flip();

        if (!isPlayerDetected)
            Walking();
        else if (isPlayerDetected && !playerIsInAttackRange())
            Running();

        if (canAttack() && isPlayerDetected && PlayerIsInFront())
            AttackCo = StartCoroutine(Attack());

    }

    private void Walking()
    {
        if (currentState != States.walking)
        {
            SetState("isWalking", true, States.walking);
        }
        if (isFacingRight)
            transform.Translate(Vector2.right * walkSpeed * Time.deltaTime);
        else
            transform.Translate(Vector2.left * walkSpeed * Time.deltaTime);

    }

    private void Running()
    {

        if (currentState != States.running)
            SetState("isRunning", true, States.running);

        if (isFacingRight)
            transform.Translate(Vector2.right * runSpeed * Time.deltaTime);
        else
            transform.Translate(Vector2.left * runSpeed * Time.deltaTime);

    }

    private bool canAttack()
    {
        return playerIsInAttackRange() && AttackCo == null;
    }

    private bool playerIsInAttackRange()
    {
        if (Mathf.Abs(transform.position.x - PlayerController.instance.transform.position.x) < attackDistance)
            return true;
        else
            return false;
    }

    private IEnumerator Attack()
    {
        if (!PlayerIsInFront())
            Flip();

        Reset_Animations();
        SetState("inCoolDown", true, States.coolDown);

        yield return new WaitForSeconds(attackCoolDown);

        Reset_Animations();
        SetTrigger("Attack", States.attacking);
        yield return new WaitForSeconds(0.4f);
        PlayerStats.instance.DamagePlayer();
        yield return new WaitForSeconds(0.5f);
        StopAttack();
    }

    private void Reset_Animations()
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("inCoolDown", false);
        animator.SetBool("isDead", false);
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Hurt");
    }

    private void SetState(string animatorVarName, bool value, States activeState)
    {
        animator.SetBool(animatorVarName, value);
        currentState = activeState;
    }
    private void SetTrigger(string triggerName, States activeState)
    {
        animator.SetTrigger(triggerName);
        currentState = activeState;
    }

    private bool shouldFlip()
    {
        if ((transform.localPosition.x > RightBound.localPosition.x && isFacingRight)
        || (transform.localPosition.x < LeftBound.localPosition.x && !isFacingRight))
            return true;
        else
            return false;
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        localScaleX = transform.localScale.x;
        localScaleX *= -1;
        transform.localScale = new Vector2(localScaleX, transform.localScale.y);
    }


    public bool PlayerIsInFront()
    {
        return (PlayerController.instance.transform.position.x > transform.position.x && isFacingRight)
             || (PlayerController.instance.transform.position.x < transform.position.x && !isFacingRight);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            isPlayerDetected = false;
            StopAttack();
            Reset_Animations();
        }
    }

    private void StopAttack()
    {
        if (AttackCo != null)
        {
            StopCoroutine(AttackCo);
            AttackCo = null;
            animator.ResetTrigger("Attack");
        }
    }

    public void DamageChomper(float damage)
    {
        currenthealth -= damage;
        if (currenthealth <= 0)
            Dead();
        else
            HurtCo = StartCoroutine(Hurt());
    }
    private void Dead()
    {
        SetState("isDead", true, States.dead);
        Destroy(transform.parent.gameObject, 1f);
    }
    private IEnumerator Hurt()
    {
        StopAttack();
        SetTrigger("Hurt", States.hurt);
        if (isFacingRight)
            rb2d.AddForce(new Vector2(-KnockBackForce, knockBackTime), ForceMode2D.Impulse);
        else
            rb2d.AddForce(new Vector2(KnockBackForce, knockBackTime), ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockBackTime);
        SetState("isCoolDown", true, States.coolDown);

    }
}

