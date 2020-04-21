using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIButtonsManager : MonoBehaviour
{


    // all functions of this script are called from inspector's onClick
    public void QuitTheGame()
    {
        Application.Quit();
    }

    public void RestartTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Ok()
    {
        UIManager.instance.LevelCompletePanel.SetActive(false);
        PlayerController.instance.gamePaused = false;


        if (UIManager.instance.LevelIncompleteText.enabled)
            UIManager.instance.LevelIncompleteText.enabled = false;

        UIManager.instance.OkButton.SetActive(false);

    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void LoadLevelSelectionScene()
    {
        SceneManager.LoadScene("LevelSelection");
    }
    public void ResumeGame()
    {
        PlayerController.instance.gamePaused = false;
        UIManager.instance.PausePanel.SetActive(false);
    }

}
