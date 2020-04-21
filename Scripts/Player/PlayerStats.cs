using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int playerHeartCount = 3;
    public int currentHeartcount;

    public bool isDead;


    public static PlayerStats instance;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }
    private void Start()
    {
        currentHeartcount = playerHeartCount;
    }

    public void DamagePlayer()
    {
        PlayerController.instance.Hurt();
        UIManager.instance.UpdateHealthUI();
    }

    public void PlayerDeath()
    {
        PlayerController.instance.Dead();
        isDead = true;
        UIManager.instance.DeathUI();
    }
}
