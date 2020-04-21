using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Texts")]
    public TextMeshProUGUI LevelCompleteText;
    public TextMeshProUGUI LevelIncompleteText;
    public TextMeshProUGUI waterCounterText;

    [Header("Images")]

    public Image GameOverImage;

    [Header("HealthUI")]
    public List<Image> ListOfHearts;
    private Image HeartToBeDisabled;

    [Header("Buttons")]
    public GameObject RestartButton;
    public GameObject OkButton;
    public GameObject NextLevelButton;

    [Header("Unity Essentials")]
    public static UIManager instance;
    public GameObject PausePanel;
    public GameObject LevelCompletePanel;


    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }
    private void Start()
    {
        if (waterCounterText)
            waterCounterText.text = waterCounterText.text + PlayerInventory.instance.collectableObject.waterDrops.ToString();
        PlayerInventory.instance.collectableObject.waterDrops = 0;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!PlayerController.instance.gamePaused)
                PasueTheGame();
            else
                ResumeTheGame();
        }
    }


    public void UpdateWaterCounterUI()
    {
        PlayerInventory.instance.collectableObject.waterDrops++;
        waterCounterText.text = "X" + PlayerInventory.instance.collectableObject.waterDrops.ToString();
    }

    public void UpdateHealthUI()
    {
        PlayerStats.instance.currentHeartcount -= 1;
        HeartToBeDisabled = ListOfHearts[PlayerStats.instance.currentHeartcount];
        HeartToBeDisabled.enabled = false;
        if (PlayerStats.instance.currentHeartcount <= 0)
        {
            PlayerStats.instance.PlayerDeath();
        }
    }
    public void DeathUI()
    {
        GameOverImage.enabled = true;
        RestartButton.SetActive(true);
    }

    public void UpdateLevelCompleteUI()
    {
        LevelCompletePanel.SetActive(true);
        PlayerController.instance.ResetAnimations();
        PlayerController.instance.gamePaused = true;
        LevelCompleteText.enabled = true;
        NextLevelButton.SetActive(true);
    }
    public void UpdateLevelIncompleteUI()
    {
        LevelCompletePanel.SetActive(true);
        PlayerController.instance.ResetAnimations();
        PlayerController.instance.gamePaused = true;
        LevelIncompleteText.enabled = true;
        OkButton.SetActive(true);
    }
    private void PasueTheGame()
    {
        PlayerController.instance.gamePaused = true;
        PausePanel.SetActive(true);
    }
    private void ResumeTheGame()
    {
        PlayerController.instance.gamePaused = false;
        PausePanel.SetActive(false);
    }

}

