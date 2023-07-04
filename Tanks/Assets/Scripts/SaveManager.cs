using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.IO;
using System;
using System.Runtime;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private GameObject buttonStart;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private ParameterManager parameterManager;
    [SerializeField] private FarmManager farmManager;
    [SerializeField] private LobbiManager lobbiManager;
    [SerializeField] private ManagerWindows managerWindows;
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private Init init;

    public void ActiveGame()
    {
        levelManager.gameObject.SetActive(true);
        parameterManager.gameObject.SetActive(true);
        farmManager.gameObject.SetActive(true);
        lobbiManager.gameObject.SetActive(true);
        managerWindows.gameObject.SetActive(true);
        settingsManager.gameObject.SetActive(true);
    }   
}


