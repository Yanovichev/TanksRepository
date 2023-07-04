using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public class PlayerData
{
    public bool isNoEmpty, isNoFirstStart;
    public string dataTime;

    public Control control;

    public int lastLevel;

    public int countAmmo, bombCount, repairCount;

    public int maxCountMiner, countMiner, rewardInPeriod, numberLevelMiner;

    public int useTankNumber;

    public string tanksParameters;
}


public enum Platform 
{
    Editor,
    Yandex, 
    VK,
    GameArter
}
public enum Control
{
    Joystick,
    Buttons,
}

public class Init : MonoBehaviour
{
    public bool pause;
    [SerializeField] private ManagerWindows managerWindows;

    public static Init Instance; 

    public Platform platform;
    [SerializeField] private GameObject gameArterPrefab;

    [Header("Mobile")]
    public bool mobile;
    public GameObject leaderboardBtn; //КНОПКА, ОТКРЫВАЮЩАЯ ЛИДЕРБОРД

    [Header("Game Scripts")]
    //ОБРАЩЕНИЕ К ДРУГИМ СКРИПТАМ ПО НАДОБНОСТИ
 
    [Header("Rewarded")]
    string rewardTag;

    [Header("Purchase")]
    string purchasedTag;
    private bool adOpen;

    [Header("Localization")]
    public string language;
    //НУЖНО ПЕРЕЧИСЛИТЬ ТЕКСТЫ, КОТОРЫЕ НЕОБХОДИМО ПЕРЕВЕСТИ
    //[SerializeField] private TextMeshProUGUI upLevelText;

    [Header("Save")]
    public PlayerData playerData;
    bool wasLoad;

    [Header("Links")]
    string developerNameYandex = "GeeKid%20-%20школа%20программирования";

    [SerializeField] private string colorDebug;

    [SerializeField] private SaveManager saveManager;

    public void ItIsMobile()
    {
        mobile = true;
        leaderboardBtn.SetActive(true);
    }

    private void Awake()
    {
        Instance = this;
        if (platform != Platform.GameArter)
        {
            Destroy(gameArterPrefab);
        }
        else
        {
            gameArterPrefab.SetActive(true);
        }

        switch (platform)
        {
            case Platform.Editor:
                ShowBannerAd();
                StartCoroutine(BannerVK());
                language = "ru"; //ВЫБРАТЬ ЯЗЫК ДЛЯ ТЕСТОВ. ru/en/tr
                Localization();

                LoadEditor();

                break;
            case Platform.Yandex:
                language = Utils.GetLang();
                if (wasLoad)
                    Utils.LoadExtern();
                Localization();
                ShowInterstitialAd();
                break;
            case Platform.VK:
                language = "ru";
                Localization();
                StartCoroutine(BannerVK());
                StartCoroutine(RewardLoad());
                StartCoroutine(InterLoad());
                if (wasLoad)
                    Utils.VK_Load();
                break;
        }
        saveManager.ActiveGame();//ДОБАВИЛ, ЗАПУСКАЕТ СКРИПТЫ КОТОРЫЕ ИСПОЛЬЗУЮТ СОХРАНЕННЫЕ ДАННЫЕ

        Debug.Log(JsonUtility.ToJson(playerData));
    }

    //РЕКЛАМА//
    IEnumerator RewardLoad()
    {
    	yield return new WaitForSeconds(15);
    	switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>REWARD LOAD</color>");
                break;
            case Platform.VK:
                Utils.VK_AdRewardCheck();
                break;
        }
    }

    IEnumerator InterLoad()
    {
    	while (true)
    	{	
    		yield return new WaitForSeconds(15);
	    	switch (platform)
	        {
	            case Platform.Editor:
	                Debug.Log($"<color={colorDebug}>INTERSTITIAL LOAD</color>");
	                break;
	            case Platform.VK:
	                Utils.VK_AdInterCheck();
	                break;
	        }
    	}
    }


    IEnumerator BannerVK()
    {
    	yield return new WaitForSeconds(5);
    	ShowBannerAd();
    }

    public void ShowInterstitialAd()
    {
        switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>INTERSTITIAL SHOW</color>");
                break;
            case Platform.Yandex:
                Utils.AdInterstitial();
                break;
            case Platform.VK:
                Utils.VK_Interstitial();
                break;
        }
    }

    public void ShowRewardedAd(string idOrTag)
    {
        switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>REWARD SHOW</color>");
                rewardTag = idOrTag;
                OnRewarded();
                break;
            case Platform.Yandex:
                rewardTag = idOrTag;
                Utils.AdReward();
                break;
            case Platform.VK:
                rewardTag = idOrTag;
                Utils.VK_Rewarded();
                break;
        }
    }

    public void ShowBannerAd()
    {
        switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>BANNER SHOW</color>");
                break;
            case Platform.VK:
                Utils.VK_Banner();
                break;
        }
    }

    public void OnRewarded()
    {
        if (rewardTag == "Rewive")
        {
            managerWindows.ReviweAd();
        }
        Debug.Log($"<color=yellow>REWARD:</color> {rewardTag}");
        StartCoroutine(RewardLoad());
    }
    //РЕКЛАМА//


    //ПАУЗА И ПРОДОЛЖЕНИЕ//
    public void StopMusAndGame()
    {
        adOpen = true;
        AudioListener.volume = 0;
        Time.timeScale = 0;
    }

    public void ResumeMusAndGame()
    {
        adOpen = false;
        AudioListener.volume = 1;
        Time.timeScale = 1;
    }
    //ПАУЗА И ПРОДОЛЖЕНИЕ//



    //ЛОКАЛИЗАЦИЯ//
    public void Localization()
    {
        if (language == "ru")
        {
            //ПЕРЕВОД НА РУССКИЙ
        }
        if (language == "en")
        {
            //ПЕРЕВОД НА АНГЛИЙСКИЙ
        }
        if (language == "tr")
        {
            //ПЕРЕВОД НА ТУРЕЦКИЙ
        }
    }
    //ЛОКАЛИЗАЦИЯ//



    //КНОПКА ДРУГИЕ ИГРЫ//
    public void OpenOtherGames()
    {
        switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>OPEN OTHER GAMES</color>");
                break;
            case Platform.Yandex:
                var domain = Utils.GetDomain();
                Application.OpenURL($"https://yandex.{domain}/games/developer?name=" + developerNameYandex);
                break;
            case Platform.VK:
            	rewardTag = "Group";
                Utils.VK_ToGroup();
                break;
        }
    }
    //КНОПКА ДРУГИЕ ИГРЫ//



    //ОТЗЫВЫ//
    public void RateGameFunc()
    {
        switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>REWIEV GAME</color>");
                break;
            case Platform.Yandex:
                Utils.RateGame();
                break;
        }
    }
    //ОТЗЫВЫ//



    //СОХРАНЕНИЕ И ЗАГРУЗКА//
    public void Save()
    {
        //ВСТАВИТЬ ДОПОЛНИТЕЛЬНОЕ СОХРАНЕНИЕ, ЕСЛИ ТРЕБУЕТСЯ

        string jsonString = "";

        switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>SAVE</color>");

                SaveEditor();

                break;
            case Platform.Yandex:
                if (wasLoad)
                {   
                    jsonString = JsonUtility.ToJson(playerData);
                    Utils.SaveExtern(jsonString);
                    Debug.Log("Save");
                }
                break;
            case Platform.VK:
                jsonString = JsonUtility.ToJson(playerData);
                Utils.VK_Save(jsonString);
                break;
        }
    }

    public void SetPlayerData(string value)
    {
        playerData = JsonUtility.FromJson<PlayerData>(value);
        //PlayerData._data = _saveData;
        wasLoad = true;
    }
    //СОХРАНЕНИЕ И ЗАГРУЗКА//



    //ВНУТРИИГРОВЫЕ ПОКУПКИ
    public void RealBuyItem(string idOrTag) //открыть окно покупки
    {
        switch (platform)
        {
            case Platform.Editor:
                purchasedTag = idOrTag;
                OnPurchasedItem();
                Debug.Log($"<color={colorDebug}>PURCHASE: </color> {purchasedTag}");
                break;
            case Platform.Yandex:
                purchasedTag = idOrTag;
                Utils.BuyItem(idOrTag);
                break;
            case Platform.VK:
                purchasedTag = idOrTag;
                Debug.Log($"<color={colorDebug}>PURCHASE VK</color>");
                break;
        }
    }

    public void CheckBuysOnStart(string idOrTag) //проверить покупки на старте
    {
        Utils.CheckBuyItem(idOrTag);
    }

    private void OnPurchasedItem() //начислить покупку (при удачной оплате)
    {
        if (purchasedTag == "purchasedID")
        {
            
        }
    }

    public void SetPurchasedItem() //начислить уже купленные предметы на старте
    {
        if (purchasedTag == "purchasedID")
        {
            
        }
    }
    //ВНУТРИИГРОВЫЕ ПОКУПКИ



    //ЛИДЕРБОРД
    public void Leaderboard(string leaderboardName, int value)
    {
        switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>SET LEADERBOARD:</color> {value}");
                break;
            case Platform.Yandex:
                Utils.SetToLeaderboard(value, leaderboardName);
                break;
            case Platform.VK:
                if (mobile)
                    Utils.VK_OpenLeaderboard(value);
                break;
        }
    }

    public void LeaderboardBtn(int value) //Для кнопки лидерборда в VK
    {
    	//value = playerData.Level;
        switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>SET LEADERBOARD:</color> {value}");
                break;
            case Platform.Yandex:
                break;
            case Platform.VK:
                Utils.VK_OpenLeaderboard(value);
                break;
        }
    }
    //ЛИДЕРБОРД


    //ЗВУК И ПАУЗА ПРИ СВОРАЧИВАНИИ
    void OnApplicationFocus(bool hasFocus)
    {
        Silence(!hasFocus);
    }

    void OnApplicationPause(bool isPaused)
    {
        Silence(isPaused);
    }

    private void Silence(bool silence)
    {
        AudioListener.volume = silence ? 0 : 1;
        Time.timeScale = silence ? 0 : 1;

        if (adOpen)
        {
            Time.timeScale = 0;
            AudioListener.volume = 0;
        }
        if (pause)
        {
            Time.timeScale = 0;
            AudioListener.volume = 0;
        }
    }
    //ЗВУК И ПАУЗА ПРИ СВОРАЧИВАНИИ


    //СОЦИАЛЬНЫЕ ФУНКЦИИ VK//
    public void ToStarGame()
    {
    	switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>GAME TO STAR</color>");
                break;
            case Platform.Yandex:
                break;
            case Platform.VK:
                Utils.VK_Star();
                break;
        }
    }

    public void ShareGame()
    {
    	switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>SHARE</color>");
                break;
            case Platform.Yandex:
                break;
            case Platform.VK:
                Utils.VK_Share();
                break;
        }
    }

    public void InvitePlayers()
    {
    	switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>INVITE</color>");
                break;
            case Platform.Yandex:
                break;
            case Platform.VK:
                Utils.VK_Invite();
                break;
        }
    }
    //СОЦИАЛЬНЫЕ ФУНКЦИИ VK//








    private void OnApplicationQuit()
    {
        SetDateTime(DateTime.UtcNow);
        Save();
    }

    public void SetDateTime(DateTime dateTime)
    {
        playerData.dataTime = dateTime.ToString("u", CultureInfo.InvariantCulture);
    }
    public DateTime GetdateTime()
    {
        if (playerData.dataTime != "")
        {
            DateTime result = DateTime.ParseExact(playerData.dataTime, "u", CultureInfo.InvariantCulture);
            return result;
        }
        return DateTime.UtcNow;
    }

    public void SaveEditor()
    {
        playerData.isNoEmpty = true;

        PlayerPrefs.SetString("isNoEmpty", playerData.isNoEmpty.ToString());
        PlayerPrefs.SetString("isNoFirstStart", playerData.isNoFirstStart.ToString());
        PlayerPrefs.SetString("dataTime", playerData.dataTime);
        PlayerPrefs.SetInt("lastLevel", playerData.lastLevel);
        PlayerPrefs.SetInt("countAmmo", playerData.countAmmo);
        PlayerPrefs.SetInt("bombCount", playerData.bombCount);
        PlayerPrefs.SetInt("repairCount", playerData.repairCount);
        PlayerPrefs.SetInt("maxCountMiner", playerData.maxCountMiner);
        PlayerPrefs.SetInt("countMiner", playerData.countMiner);
        PlayerPrefs.SetInt("rewardInPeriod", playerData.rewardInPeriod);
        PlayerPrefs.SetInt("numberLevelMiner", playerData.numberLevelMiner);
        PlayerPrefs.SetInt("useTankNumber", playerData.useTankNumber);
        PlayerPrefs.SetString("tanksParameters", playerData.tanksParameters);
        if (playerData.control == Control.Buttons)
        {
            PlayerPrefs.SetString("control", "Button");
        }
        else
        {
            PlayerPrefs.SetString("control", "Joystick");
        }
        PlayerPrefs.Save();

        //File.WriteAllText(Application.streamingAssetsPath + "/SaveData.json", JsonUtility.ToJson(playerData));
        //Debug.Log("Save");
    }
    public void LoadEditor()
    {
        if (PlayerPrefs.HasKey("isNoEmpty") && Convert.ToBoolean(PlayerPrefs.GetString("isNoEmpty")))
        {
            playerData.isNoEmpty = Convert.ToBoolean(PlayerPrefs.GetString("isNoEmpty"));
            playerData.isNoFirstStart = Convert.ToBoolean(PlayerPrefs.GetString("isNoFirstStart"));
            playerData.dataTime = PlayerPrefs.GetString("dataTime");
            playerData.countAmmo = PlayerPrefs.GetInt("countAmmo");
            playerData.bombCount = PlayerPrefs.GetInt("bombCount");
            playerData.repairCount = PlayerPrefs.GetInt("repairCount");
            playerData.maxCountMiner = PlayerPrefs.GetInt("maxCountMiner");
            playerData.countMiner = PlayerPrefs.GetInt("countMiner");
            playerData.rewardInPeriod = PlayerPrefs.GetInt("rewardInPeriod");
            playerData.numberLevelMiner = PlayerPrefs.GetInt("numberLevelMiner");
            playerData.useTankNumber = PlayerPrefs.GetInt("useTankNumber");
            playerData.lastLevel = PlayerPrefs.GetInt("lastLevel");
            playerData.tanksParameters = PlayerPrefs.GetString("tanksParameters");
            string controlString = PlayerPrefs.GetString("control");
            if (controlString == "Button")
            {
                playerData.control = Control.Buttons;
            }
            else if (controlString == "Joystick")
            {
                playerData.control = Control.Joystick;
            }
            else
            {
                playerData.control = Control.Joystick;
            }
        }
        //if (File.Exists(Application.streamingAssetsPath + "/SaveData.json"))
        //{
        //    playerData = JsonUtility.FromJson<PlayerData>(File.ReadAllText(Application.streamingAssetsPath + "/SaveData.json"));
        //    Debug.Log("Load");
        //}
        //else
        //{  
        //    Debug.Log("Don't load");
        //}
    }
}

