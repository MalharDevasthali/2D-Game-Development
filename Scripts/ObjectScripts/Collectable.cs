using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
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
                UIManager.instance.UpdateWaterCounterUI();
                Destroy(this.gameObject);
            }
        }
    }
}
