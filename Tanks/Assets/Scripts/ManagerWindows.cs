using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManagerWindows : MonoBehaviour
{
    public GameObject nextLevelButton;
    [SerializeField] private GameObject playingPanel, pausePanel, lobbiPanel, gameOverPanel, selectPanel, victoryPanel, minerPanel, showLevelText, startPanel, marketPanel, joystick, settingsPanel;
    [SerializeField] private TMP_Text pauseText;
    [SerializeField] private GameObject pauseButton, buttonSettings, buttonsMobile, buttonFire;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private LobbiManager lobbiManager;
    [SerializeField] private FarmManager farmManager;
    [SerializeField] private Init init;

    private void Awake()
    {
        if (Init.Instance.mobile)
        {
            buttonSettings.SetActive(true);
        }
        else
        {
            buttonSettings.SetActive(false);
        }
    }
    private void CloseAll()
    {
        lobbiPanel.SetActive(false);
        playingPanel.SetActive(false);
        pausePanel.SetActive(false);
        pauseButton.SetActive(false);
        gameOverPanel.SetActive(false);
        selectPanel.SetActive(false);
        victoryPanel.SetActive(false);
        minerPanel.SetActive(false);
        showLevelText.SetActive(false);
        startPanel.SetActive(false);
        OffMobilePlaying();
    }
    private void OnMobilePlaying()
    {
        if (Init.Instance.mobile)
        {
            buttonFire.SetActive(true);
            if (Init.Instance.playerData.control == Control.Joystick)
            {
                joystick.SetActive(true);
                buttonFire.GetComponent<RectTransform>().anchoredPosition = new Vector3(-275, 157f, 0f);
            } 
            else
            {
                buttonsMobile.SetActive(true);
                buttonFire.GetComponent<RectTransform>().anchoredPosition = new Vector3(-270f, 289f, 0f);
            }               
        }  
    }
    private void OffMobilePlaying()
    {
        if (Init.Instance.mobile)
        {
            buttonFire.SetActive(false);
            if (Init.Instance.playerData.control == Control.Joystick) joystick.SetActive(false);
            else buttonsMobile.SetActive(false);
        }
    }
    public void OpenSettingsPanel()
    {
        settingsPanel.SetActive(true);
        lobbiManager.DeleteLobbiTank();
    }
    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
        lobbiManager.FirstSpawn();
    }
    public void ToSelectLevel()
    {
        CloseAll();
        selectPanel.SetActive(true);
    }
    public void ToVictoryPanel()
    {
        CloseAll();
        victoryPanel.SetActive(true);
        lobbiManager.OffPlayer();
    }
    public void ToLobbi()
    {
        CloseAll();
        lobbiPanel.SetActive(true);
        playingPanel.SetActive(true);
    }
    public void Play()
    {
        CloseAll();
        playingPanel.SetActive(true);
        pauseButton.SetActive(true);
        showLevelText.SetActive(true);
        OnMobilePlaying();
        if (Init.Instance.language == "ru")
            showLevelText.GetComponent<TMP_Text>().text = $"Уровень {levelManager.useLevelNumber}";
        levelManager.GoGame();
    }
    public void ToMiner()
    {
        CloseAll();
        minerPanel.SetActive(true);
    }
    public void GameOver()
    {
        CloseAll();
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }
    public void Risen()
    {
        Init.Instance.ShowRewardedAd("Rewive");

    }

    public void ReviweAd()
    {
        CloseAll();
        playingPanel.SetActive(true);
        lobbiManager.useTankObjInGame.GetComponent<Player>().HillRise();
        OnMobilePlaying();
    }

    public void EndGame()
    {
        CloseAll();
        lobbiManager.OnPlayer();
        Time.timeScale = 1f;
        lobbiManager.DestroyGameTank();
        levelManager.ClearGameSpace();
        lobbiPanel.SetActive(true);
    }
    public void Continue()
    {
        Init.Instance.pause = false;
        CloseAll();
        lobbiManager.OnPlayer();
        Time.timeScale = 1f;
        playingPanel.SetActive(true);
        pauseButton.SetActive(true);
        OnMobilePlaying();
    }

    public void Pause()
    {
        Init.Instance.pause = true;
        CloseAll();
        playingPanel.SetActive(true);
        pausePanel.SetActive(true);
        if (Init.Instance.language == "ru")
            pauseText.text = $"Пауза\nУровень {levelManager.useLevelNumber}";
        lobbiManager.OffPlayer();
        Time.timeScale = 0f;
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        lobbiManager.OnPlayer();
        lobbiManager.DestroyGameTank();
        levelManager.ClearGameSpace();
        lobbiManager.Play();
        OnMobilePlaying();
        
    }
    public void NextLevel()
    {
        CloseAll();
        lobbiManager.OnPlayer();
        init.ShowInterstitialAd();
        playingPanel.SetActive(true);
        pauseButton.SetActive(true);
        lobbiManager.DestroyGameTank();
        levelManager.ClearGameSpace();
        levelManager.NextLevel();
        lobbiManager.Play();
    }
    public void ToMarket()
    {
        marketPanel.SetActive(true);
        lobbiManager.DeleteLobbiTank();
        farmManager.DeleteTank();
    }
    public void ExitMarket()
    {
        marketPanel.SetActive(false);
        lobbiManager.FirstSpawn();
        farmManager.SpawnTank();
    }
}
