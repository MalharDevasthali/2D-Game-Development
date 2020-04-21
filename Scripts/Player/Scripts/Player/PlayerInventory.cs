using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    public struct Collectables
    {
        public int waterDrops;
        public int keys;
        //add here other collectables as per need...:) 
    }

    public struct Weapons
    {
        public bool isStaffUnlocked;
        public bool isGunUnlocked;
    }

    public Weapons weapons;
    public Collectables collectableObject;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        collectableObject.waterDrops = 0;
        collectableObject.keys = 0;
        weapons.isStaffUnlocked = true;
    }
    public int GetWater()
    {
        return collectableObject.waterDrops;
    }
    public int GetKeys()
    {
        return collectableObject.keys;
    }




}
