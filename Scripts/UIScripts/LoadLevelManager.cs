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
    private string currentScene;
    private int levelReached;
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        SceneManager.activeSceneChanged += ActiveSceneChanged;
        levelReached = PlayerPrefs.GetInt("levelReached", 1); // 1 will be unlocked as default

        UpdateLevelUnlocked();

    }

    private void UpdateLevelUnlocked()
    {

        for (int i = 0; i < LevelList.Count; i++)
        {
            if (i + 1 > levelReached)
                LevelList[i].gameObject.GetComponent<Button>().interactable = false;
            else
                LevelList[i].gameObject.GetComponent<Button>().interactable = true; // imp  for activating buttons in Game
        }
    }

    private void ActiveSceneChanged(Scene current, Scene next)
    {
        if (next.name.CompareTo("LevelSelection") != 0)
        {
            LevelHolder.SetActive(false);
        }
        else
        {
            LevelHolder.SetActive(true);
        }
        if (PlayerController.instance != null)
            PlayerController.instance.gamePaused = false;
    }
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

        if (LevelList[GetCurrentLevel() - 1].isLevelComplete()) //list index starts from 0 and build index of level 1 is 1..hence -1
        {
            UnlockNextLevel();
            UIManager.instance.UpdateLevelCompleteUI();
        }
        else
        {
            UIManager.instance.UpdateLevelIncompleteUI();
        }
    }

    private int GetCurrentLevel()
    {
        return currentLevel = SceneManager.GetActiveScene().buildIndex;
    }
    private void UnlockNextLevel()
    {
        PlayerPrefs.SetInt("levelReached", GetCurrentLevel() + 1);
        levelReached = PlayerPrefs.GetInt("levelReached"); //this line is imp for updating prefs dynamically in Game
        UpdateLevelUnlocked();
    }
}
