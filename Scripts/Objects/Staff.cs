using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    public float StaffDamage;
    public float staffAttackCooldown;
    private bool canAttack = true;
    private Coroutine DamageCo;
    private void Update()
    {
        if (PlayerInventory.instance.weapons.isStaffUnlocked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlayerController.instance.Attack();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.GetComponent<Chomper>() != null)
        {
            if (PlayerController.instance.state == PlayerController.AnimState.Attacking && canAttack)
            {
                if (DamageCo == null)
                    DamageCo = StartCoroutine(GiveDamage(other));
            }
        }
    }

    private IEnumerator GiveDamage(Collider2D other)
    {
        other.gameObject.GetComponent<Chomper>().DamageChomper(StaffDamage);
        canAttack = false;
        yield return new WaitForSeconds(staffAttackCooldown);
        canAttack = true;
        if (DamageCo != null)
            StopDamageCo();
    }
    private void StopDamageCo()
    {
        StopCoroutine(DamageCo);
        DamageCo = null;
    }
}

