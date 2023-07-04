using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class FarmManager : MonoBehaviour
{
    [SerializeField] private LevelMiner[] levelsMiner;
    [SerializeField] private GameObject buttonUpMiner, tankInMiner, prefabTank;
    [SerializeField] private Transform targetTank;
    [SerializeField] private TMP_Text countMinerText, buttonUpText;
    [SerializeField] private ParameterManager parameterManager;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Init init;

    public int NumberLevelMiner
    {
        get { return init.playerData.numberLevelMiner; }
        set
        {
            init.playerData.numberLevelMiner = value;
            init.Save();
            Debug.Log("Save parameters");
        }
    }
    public int RewardInPeriod
    {
        get { return init.playerData.rewardInPeriod; }
        set
        {
            init.playerData.rewardInPeriod = value;
            init.Save();
            Debug.Log("Save parameters");
        }
    }
    public int MaxCountMiner
    {
        get { return init.playerData.maxCountMiner; }
        set
        {
            init.playerData.maxCountMiner = value;
            countMinerText.text = $"{CountMiner} / {MaxCountMiner}";
            init.Save();
            Debug.Log("Save parameters");
        }
    }
    public int CountMiner
    {
        get { return init.playerData.countMiner; }
        set
        {
            if (value >= MaxCountMiner)
            {
                init.playerData.countMiner = MaxCountMiner;
                
            }
            else if (value >= 0 && value < MaxCountMiner)
            {
                init.playerData.countMiner = value;
            }
            countMinerText.text = $"{CountMiner}/{MaxCountMiner}";
            init.Save();
            Debug.Log("Save parameters");
        }
    }
    void Start()
    {
        RewardOffline();
        InvokeRepeating("OnlineFarm", 1f, 5f);
        UpdateMinerUp();
    }
    public void SpawnTank()
    {
        tankInMiner = Instantiate(prefabTank, targetTank);  
    }
    public void DeleteTank()
    {
        Destroy(tankInMiner);
    }
    public void UpdateMinerUp()
    {
        if (NumberLevelMiner == levelsMiner.Length)
        {
            buttonUpMiner.SetActive(false);
            buttonUpText.gameObject.SetActive(false);
        }
        else if (parameterManager.AmmoCount >= levelsMiner[NumberLevelMiner - 1].costUp && levelManager.LastLevel > levelsMiner[NumberLevelMiner - 1].levelUp)
        {
            buttonUpMiner.SetActive(true);
            buttonUpMiner.GetComponent<Image>().color = new Color(255f, 255f, 255f, 1f);
            Button buttonUp = buttonUpMiner.GetComponent<Button>();
            buttonUp.onClick.RemoveAllListeners();
            buttonUp.onClick.AddListener(UpLevelMiner);
            if (Init.Instance.language == "ru")
                buttonUpText.text = $"Улучшить шахту\nТребования\nЗолото {levelsMiner[NumberLevelMiner - 1].costUp}\nПройти уровень {levelsMiner[NumberLevelMiner - 1].levelUp}";
        }
        else
        {
            buttonUpMiner.SetActive(true);
            buttonUpMiner.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0.5f);
            Button buttonUp = buttonUpMiner.GetComponent<Button>();
            buttonUp.onClick.RemoveAllListeners();
            if (Init.Instance.language == "ru")
                buttonUpText.text = $"Улучшить шахту\nТребования\nЗолото {levelsMiner[NumberLevelMiner - 1].costUp}\nПройти уровень {levelsMiner[NumberLevelMiner - 1].levelUp}";
        }
    }
    private void UpLevelMiner()
    {
        NumberLevelMiner++;
        LevelMiner useLevelMiner = levelsMiner[NumberLevelMiner - 1];
        RewardInPeriod = useLevelMiner.rewardInPeriod;
        MaxCountMiner = useLevelMiner.maxCount;
        UpdateMinerUp();
    }
    private void RewardOffline()
    {
        DateTime lastSaveTime = init.GetdateTime();
        TimeSpan timePassed = DateTime.UtcNow - lastSaveTime;
        Debug.Log($"lastSaveTime {lastSaveTime}, now time {DateTime.UtcNow}");
        int secondPassed = (int)timePassed.TotalSeconds;
        CountMiner += (secondPassed / 5) * RewardInPeriod;
    }
    public void TakeMinerGold()
    {
        parameterManager.AmmoCount += CountMiner;
        CountMiner = 0;
    }
    private void OnlineFarm()
    {
        CountMiner += RewardInPeriod;
    }
}
