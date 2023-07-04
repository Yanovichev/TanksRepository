using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private LobbiManager lobbiManager;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private Init init;
    [SerializeField] private GameObject firstTutorialPanel, healthTutorialPanel, coinsTutorialPanel, bonusTutorialPanel, enemyTutorialPanel, controlTutorialPanel, voctoryTutorialPanel, buyBonusTutorialPanel, upgradeTutorialPanel,
        mineTutorialPanel;
    [SerializeField] private GameObject areaHealthTutorial, areaCoinsTutorial, areaBonusTutorial, areaEnemyTutorial, areaBuyBonusTutorial, areaUpgradeTutorial, areaMineTutorial;
    [SerializeField] private GameObject fingerHealthTutorial, fingerCoinsTutorial, fingerBonusTutorial, fingerEnemyTutorial, fingerBuyBonusTutorial, fingerUpgradeTutorial, fingerMineTutorial;
    [SerializeField] private GameObject menuButtom;
    [SerializeField] private GameObject backGround;
    public void CloseAll()
    {
        voctoryTutorialPanel.SetActive(false);
        firstTutorialPanel.SetActive(false);
        healthTutorialPanel.SetActive(false);
        areaHealthTutorial.SetActive(false);
        coinsTutorialPanel.SetActive(false);
        areaCoinsTutorial.SetActive(false);
        bonusTutorialPanel.SetActive(false);
        areaBonusTutorial.SetActive(false);
        enemyTutorialPanel.SetActive(false);
        areaEnemyTutorial.SetActive(false);
        controlTutorialPanel.SetActive(false);
        fingerHealthTutorial.SetActive(false);
        fingerCoinsTutorial.SetActive(false);
        fingerBonusTutorial.SetActive(false);
        fingerEnemyTutorial.SetActive(false);
        buyBonusTutorialPanel.SetActive(false);
        areaBuyBonusTutorial.SetActive(false);
        fingerBuyBonusTutorial.SetActive(false);
        upgradeTutorialPanel.SetActive(false);
        areaUpgradeTutorial.SetActive(false);
        fingerUpgradeTutorial.SetActive(false);
        mineTutorialPanel.SetActive(false);
        areaMineTutorial.SetActive(false);
        fingerMineTutorial.SetActive(false);
    }
    public void FirstTutorial()
    {
        CloseAll();
        backGround.SetActive(true);
        lobbiManager.OnGodMode();
        menuButtom.SetActive(false);
        firstTutorialPanel.SetActive(true);
    }
    public void OKFirstTutorial()
    {
        OpenHealthTutorial();
    }
    public void OKHealthTutorial()
    {
        OpenCoinsTutorial();
    }
    public void OKCoinsTutorial()
    {
        OpenBonusTutorial();
    }
    public void OKBonusTutorial()
    {
        OpenEnemyTutorial();
    }
    public void OKEnemyTutorial()
    {
        OpenControlTutorial();
    }
    public void OKControlTutorial()
    {
        FirstBattle();
    }
    public void OKVictoryTutorial()
    {
        OpenBuyBonusTutorial();
        menuButtom.SetActive(true);
    }
    public void OKBuyBonusTutorial()
    {
        OpenUpgradeTutorial();
    }
    public void OKUpgradeTutorial()
    {
        OpenMineTutorial();
    }
    public void OKMineTutorial()
    {
        CloseAll();
        backGround.SetActive(false);
        init.playerData.isNoFirstStart = true;
        init.Save();

    }
    public void OpenVictoryTutorial()
    {
        CloseAll();
        lobbiManager.OffPlayer();
        backGround.SetActive(true);
        voctoryTutorialPanel.SetActive(true);
    }
    private void OpenHealthTutorial()
    {
        CloseAll();
        fingerHealthTutorial.SetActive(true);
        healthTutorialPanel.SetActive(true);
        areaHealthTutorial.SetActive(true);
    }
    private void OpenCoinsTutorial()
    {
        CloseAll();
        fingerCoinsTutorial.SetActive(true);
        coinsTutorialPanel.SetActive(true);
        areaCoinsTutorial.SetActive(true);
    }
    private void OpenBonusTutorial()
    {
        CloseAll();
        fingerBonusTutorial.SetActive(true);
        bonusTutorialPanel.SetActive(true);
        areaBonusTutorial.SetActive(true);
    }
    private void OpenEnemyTutorial()
    {
        CloseAll();
        fingerEnemyTutorial.SetActive(true);
        enemyTutorialPanel.SetActive(true);
        areaEnemyTutorial.SetActive(true);
    }
    private void OpenControlTutorial()
    {
        CloseAll();
        controlTutorialPanel.SetActive(true);
    }
    private void FirstBattle()
    {
        CloseAll();
        lobbiManager.OnPlayer();
        backGround.SetActive(false);
    }
    private void OpenBuyBonusTutorial()
    {
        CloseAll();
        buyBonusTutorialPanel.SetActive(true);
        areaBuyBonusTutorial.SetActive(true);
        fingerBuyBonusTutorial.SetActive(true);
    }
    private void OpenUpgradeTutorial()
    {
        CloseAll();
        upgradeTutorialPanel.SetActive(true);
        areaUpgradeTutorial.SetActive(true);
        fingerUpgradeTutorial.SetActive(true);
    }
    public void OpenMineTutorial()
    {
        CloseAll();
        mineTutorialPanel.SetActive(true);
        areaMineTutorial.SetActive(true);
        fingerMineTutorial.SetActive(true);
    }
}
