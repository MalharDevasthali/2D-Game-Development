using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevelManager : MonoBehaviour
{


    public List<Level> LevelList;
    private int currentLevel;
    public GameObject LevelHolder;
    //public Sprite _highlightedSprite;

    public static LoadLevelManager instance;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }
    public void LevelCheck()
    {
        if (LevelList[GetCurrentLevelIndex()].isLevelComplete())
        {
            UnlockNextLevel();
            // LoadNextLevel();
            UIManager.instance.UpdateLevelCompleteUI();
        }
        else
        {
            UIManager.instance.UpdateLevelIncompleteUI();
        }
    }

    private int GetCurrentLevelIndex()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        return currentLevel - 1;
    }

    private void UnlockNextLevel()
    {
        LevelList[GetCurrentLevelIndex()].gameObject.GetComponent<Button>().enabled = true;
    }
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(GetCurrentLevelIndex() + 2);
    }
    //calling from onClick event via Inspector
    public void LoadLevel(int index)
    {
        LevelHolder.SetActive(false);
        SceneManager.LoadScene(index);
    }

}
