using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [System.Serializable]

    public class Clipping
    {
        public enum Collectables
        {
            waterDrops, keys
        }
        public Collectables collectableType;
        public int requirement;
    }
    [HideInInspector]
    public Clipping clipping;
    private Button thisLevelButton;

    [Header(" Level Info")]
    [SerializeField] private int thisLevelIndex;
    [SerializeField] private List<Clipping> WinCondition;


    private void Start()
    {
        thisLevelButton = this.gameObject.GetComponent<Button>();
        thisLevelButton.onClick.AddListener(LoadLevel);
    }

    public bool isLevelComplete()
    {
        for (int i = 0; i < WinCondition.Count; i++)
        {
            if (WinCondition[i].collectableType == Clipping.Collectables.waterDrops)
            {
                if (PlayerInventory.instance.GetWater() >= WinCondition[i].requirement)
                    continue;
                else
                    return false;
            }
            if (WinCondition[i].collectableType == Clipping.Collectables.keys)
            {
                if (PlayerInventory.instance.GetKeys() >= WinCondition[i].requirement)
                    continue;
                else
                    return false;
            }
        }
        return true;
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene(thisLevelIndex);
    }
}
