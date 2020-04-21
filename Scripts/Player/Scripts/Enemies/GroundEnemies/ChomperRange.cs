using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperRange : MonoBehaviour
{
    public Chomper chomper;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            if (!PlayerStats.instance.isDead)
                chomper.isPlayerDetected = true;
            else
                chomper.isPlayerDetected = false;

            if (!chomper.PlayerIsInFront())
                chomper.Flip();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            chomper.isPlayerDetected = false;

        }
    }
}