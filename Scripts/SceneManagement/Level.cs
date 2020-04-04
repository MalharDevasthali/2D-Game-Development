using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Level : MonoBehaviour
{
    // public struct Conditions
    // {
    //     public enum ObjectsToCollect
    //     {
    //         waterDrops, keys,
    //     }
    //     public ObjectsToCollect[] objects;
    //     public int Requirements;
    // }
    // public Conditions show;
    public enum ObjectsToCollect
    {
        waterDrops, keys
    }
    public ObjectsToCollect[] objects = new ObjectsToCollect[1];

    public int waterDropsNeeded;
    public int keysNeeded;


    public bool isLevelComplete()
    {
        if (PlayerInventory.instance.collectableObject.waterDrops >= waterDropsNeeded &&
           PlayerInventory.instance.collectableObject.keys >= keysNeeded)
            return true;
        return false;
        //if (ObjectsToCollect.waterDrops >= PlayerInventory.instance.GetWater((typeof(ObjectsToCollect.waterDrops))
        //    return false;
    }


}
