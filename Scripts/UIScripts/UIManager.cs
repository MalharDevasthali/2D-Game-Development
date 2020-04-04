using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Texts")]
    public TextMeshProUGUI LevelCompleteUI;
    public TextMeshProUGUI LevelIncompleteUI;
    public TextMeshProUGUI waterCounterText;

    [Header("Images")]
    public Image LevelPanel;
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
    }
    public void DeathUI()
    {
        GameOverImage.enabled = true;
        RestartButton.SetActive(true);
    }

    public void UpdateLevelCompleteUI()
    {
        LevelPanel.enabled = true;
        LevelCompleteUI.enabled = true;
        NextLevelButton.SetActive(true);

    }

    public void UpdateLevelIncompleteUI()
    {
        LevelPanel.enabled = true;
        LevelIncompleteUI.enabled = true;
        OkButton.SetActive(true);
    }


}

