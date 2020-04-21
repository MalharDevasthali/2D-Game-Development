using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public enum ThisCollectable
    {
        waterDrops, keys,
    }
    public ThisCollectable thisCollectable;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            if (thisCollectable == ThisCollectable.waterDrops)
            {
                GetComponent<BoxCollider2D>().enabled = false;
                animator.SetTrigger("Collected");
                UIManager.instance.UpdateWaterCounterUI();
                Destroy(this.gameObject, 1.5f);
            }
        }
    }
}
