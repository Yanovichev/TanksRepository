using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbiManager : MonoBehaviour
{
    //Исправить
    [SerializeField] private ParameterManager parameterManager;
    [SerializeField] private ManagerWindows managerWindows;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private Init init;
    [SerializeField] public List<Tank> tanks;
    [SerializeField] public int maxHealthInGame, maxArmorInGame, maxDamageInGame;
    [SerializeField] private float maxSpeedInGame;
    [SerializeField] private Transform targetTank;
    [SerializeField] private GameObject buttonNext, buttonBack, buttonUpHealth, buttonUpDamage, buttonUpArmor, buttonUpSpeed, buttonBuy, buttonPlay, buttonSelectLevel, content, buttonNextSelectLevel, buttonBackSelectLevel;
    [SerializeField] private TMP_Text textUpHealth, textUpArmor, textUpDamage, textUpSpeed, textBuy;
    [SerializeField] private TMP_Text textHealthPoint, textArmorPoint, textDamagePoint, textSpeedPoint;
    public GameObject useTankObjInLobbi, useTankObjInGame;
    [SerializeField] private Tank useTank;
    [SerializeField] private Image overHealthIndicator, overArmorIndicator, overDamageIndicator, overSpeedIndicator, healthIndicator, armorIndicator, damageIndicator, speedIndicator, nextUpHealth, nextUpArmor, nextUpSpeedMove, nextUpDamage;
    public bool godMode;

    public int UseTankNumber
    {
        get { return init.playerData.useTankNumber; }
        set
        {
            init.playerData.useTankNumber = value;
            init.Save();
            Debug.Log("Save parameters");
        }
    }
    private void Awake()
    {
        if (init.playerData.tanksParameters != null && init.playerData.tanksParameters != "") LoadTanksParameters();
        FirstSpawn();
        if (!init.playerData.isNoFirstStart)
        {
            PlayNumberLevel(1);
            tutorialManager.FirstTutorial();
            OffPlayer();
        }
    }
    public void OnGodMode()
    {
        useTankObjInGame.GetComponent<Player>().godMode = true;
        godMode = true;
    }
    public void OffGodMode()
    {
        useTankObjInGame.GetComponent<Player>().godMode = false;
        godMode = false;
    }
    public void OffPlayer()
    {
        useTankObjInGame.GetComponent<MoveManager>().enabled = false;
        useTankObjInGame.GetComponent<Player>().enabled = false;
        useTankObjInGame.GetComponentInChildren<FireManager>().enabled = false;
        useTankObjInGame.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
    }
    public void OnPlayer()
    {
        useTankObjInGame.GetComponent<MoveManager>().enabled = true;
        useTankObjInGame.GetComponent<Player>().enabled = true;
        useTankObjInGame.GetComponentInChildren<FireManager>().enabled = true;
    }
    public void FirstSpawn()
    {
        if (useTankObjInLobbi == null)
        {
            SpawnInLobbi(UseTankNumber - 1);
        }
        CheckTank();
    }
    public void DeleteLobbiTank()
    {
        Destroy(useTankObjInLobbi);
        useTankObjInLobbi = null;
    }
    public void SelectTank(bool next)
    {
        Destroy(useTankObjInLobbi);
        if (next)
        {
            UseTankNumber++;
            SpawnInLobbi(UseTankNumber - 1);
        }
        if (!next)
        {
            UseTankNumber--;
            SpawnInLobbi(UseTankNumber - 1);
        }
        CheckTank();
    }
    public void CheckTank()
    {
        if (UseTankNumber == tanks.Count)
        {
            buttonNext.SetActive(false);
        }
        if (UseTankNumber == 1)
        {
            buttonBack.SetActive(false);
        }
        if (UseTankNumber < tanks.Count)
        {
            buttonNext.SetActive(true);
        }
        if (UseTankNumber > 1)
        {
            buttonBack.SetActive(true);
        }
        if (useTank.isBuy)
        {
            buttonBuy.SetActive(false);
            UpdateUpper();
            OnButtonPlay();
            OnSelectLevelButton();
        }
        else
        {
            OnBuyButton();
            offButtonPlay();
            OffSelectLevelButton();
        }
        UpdateIndicator();
    }
    public void SelectLevel(int startLevel)
    {
        LevelMarker[] levelsMarkers = content.GetComponentsInChildren<LevelMarker>();
        for (int i = 0; i < levelsMarkers.Length; i++)
        {
            levelsMarkers[i].block.SetActive(true);
            levelsMarkers[i].button.GetComponentInChildren<TMP_Text>().text = "";
            levelsMarkers[i].button.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        for (int i = 0; i < levelsMarkers.Length; i++)
        {
            int resultIndexLevel = startLevel + i;
            if (resultIndexLevel < levelManager.LastLevel)
            {
                levelsMarkers[i].block.SetActive(false);
                levelsMarkers[i].button.GetComponentInChildren<TMP_Text>().text = $"{resultIndexLevel + 1}";
                levelsMarkers[i].numberLevel = resultIndexLevel + 1;
                levelsMarkers[i].button.GetComponent<Button>().onClick.AddListener(levelsMarkers[i].PlayNemberLevel);
            }
        }

        buttonBackSelectLevel.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonNextSelectLevel.GetComponent<Button>().onClick.RemoveAllListeners();

        if (startLevel < 1)
        {
            buttonBackSelectLevel.SetActive(false);
        }
        else
        {
            buttonBackSelectLevel.SetActive(true);
            buttonBackSelectLevel.GetComponent<Button>().onClick.AddListener(delegate { SelectLevel(startLevel - levelsMarkers.Length); });
        }

        if (startLevel + levelsMarkers.Length == levelManager.levels.Count)
        {
            buttonNextSelectLevel.SetActive(false);
        }
        else
        {
            buttonNextSelectLevel.SetActive(true);
            buttonNextSelectLevel.GetComponent<Button>().onClick.AddListener(delegate { SelectLevel(startLevel + levelsMarkers.Length); });
        }    
    }
    public void DestroyGameTank()
    {
        Destroy(useTankObjInGame);
    }
    public void PlayNumberLevel(int levelNumber)
    {
        levelManager.useLevelNumber = levelNumber;
        Play();
    }
    public void PlayLastLevel()
    {
        levelManager.useLevelNumber = levelManager.LastLevel;
        Play();
    }
    public void Play()
    {
        useTankObjInGame = Instantiate(useTank.prefab);
        Player player = useTankObjInGame.GetComponent<Player>();
        player.MaxHealth = useTank.maxHealth;
        player.Armor = useTank.armor;
        FireManager fireManager = useTankObjInGame.GetComponentInChildren<FireManager>();
        fireManager.Damage = useTank.damage;
        MoveManager moveManager = useTankObjInGame.GetComponent<MoveManager>();
        moveManager.SpeedMove = useTank.speedMove;
        player.SetStartParameters();
        managerWindows.Play();
    }
    private void OnSelectLevelButton()
    {
        Button select = buttonSelectLevel.GetComponent<Button>();
        select.onClick.RemoveAllListeners();
        select.onClick.AddListener(managerWindows.ToSelectLevel);
        select.onClick.AddListener(delegate { SelectLevel(0); });
        buttonSelectLevel.GetComponent<Image>().color = new Color(255f, 255f, 255f, 1f);
    }
    private void OffSelectLevelButton()
    {
        Button select = buttonSelectLevel.GetComponent<Button>();
        select.onClick.RemoveAllListeners();
        buttonSelectLevel.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0.5f);
    }
    private void OnButtonPlay()
    {
        Button play = buttonPlay.GetComponent<Button>();
        play.onClick.RemoveAllListeners();
        play.onClick.AddListener(PlayLastLevel);
        play.GetComponent<Image>().color = new Color(255f, 255f, 255f, 1f);
    }
    private void offButtonPlay()
    {
        Button play = buttonPlay.GetComponent<Button>();
        play.onClick.RemoveAllListeners();
        play.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0.5f);
    }
    public void OnBuyButton()
    {
        buttonUpHealth.SetActive(false);
        buttonUpArmor.SetActive(false);
        buttonUpDamage.SetActive(false);
        buttonUpSpeed.SetActive(false);
        buttonBuy.SetActive(true);
        Button buyButton = buttonBuy.GetComponent<Button>();
        textBuy.text = $"{useTank.cost}";
        buyButton.onClick.RemoveAllListeners();
        if (useTank.cost <= parameterManager.AmmoCount)
        {
            buyButton.onClick.AddListener(Buy);
            buttonBuy.GetComponent<Image>().color = new Color(255f, 255f, 255f, 1f);
        }
        else
        {
            buttonBuy.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0.5f);
        }
    }
    public void Buy()
    {
        buttonBuy.SetActive(false);
        useTank.isBuy = true;
        parameterManager.AmmoCount -= useTank.cost;      
        UpdateUpper();
        SaveTanksParameters();
        Debug.Log("Save parameters");
    }
    public void UpHealth()
    {
        parameterManager.AmmoCount -= (useTank.healthPoint + 1) * 200;
        useTank.healthPoint++;
        useTank.maxHealth += useTank.HealthUp;
        if (useTank.healthPoint == useTank.maxHealthPoint)
        {
            useTank.isMaxHealth = true;
        }
        UpdateUpper();
        UpdateHealthIndicator();
        SaveTanksParameters();
        Debug.Log("Save parameters");
    }
    public void UpArmor()
    {
        parameterManager.AmmoCount -= (useTank.armorPoint + 1) * 200;
        useTank.armorPoint++;
        useTank.armor += useTank.armorUp;
        if (useTank.armorPoint == useTank.maxArmorPoint)
        {
            useTank.isMaxArmor = true;
        }
        UpdateUpper();
        UpdateArmorIndicator();
        SaveTanksParameters();
        Debug.Log("Save parameters");
    }
    public void UpDamage()
    {
        parameterManager.AmmoCount -= (useTank.damagePoint + 1) * 200;
        useTank.damagePoint++;
        useTank.damage += useTank.damageUp;
        if (useTank.damagePoint == useTank.maxDamagePoint)
        {
            useTank.isMaxDamage = true;
        }
        UpdateUpper();
        UpdateDamageIndicator();
        SaveTanksParameters();
        Debug.Log("Save parameters");
    }
    public void UpSpeed()
    {
        parameterManager.AmmoCount -= (useTank.speedPoint + 1) * 200;
        useTank.speedPoint++;
        useTank.speedMove += useTank.speedMoveUp;
        if (useTank.speedPoint == useTank.maxSpeedMovePoint)
        {
            useTank.isMaxSpeedMove = true;
        }
        UpdateUpper();
        UpdateSpeedIndicator();
        SaveTanksParameters();
        Debug.Log("Save parameters");
    }
    private void SpawnInLobbi(int index)
    {
        useTankObjInLobbi = Instantiate(tanks[index].prefabLobbi, targetTank);
        useTank = tanks[index];
    }
    public void UpdateUpper()
    {
        UpdateHealth();
        UpdateArmor();
        UpdateDamage();
        UpdateSpeed();
    }
    public void UpdateIndicator()
    {
        UpdateHealthIndicator();
        UpdateArmorIndicator();
        UpdateDamageIndicator();
        UpdateSpeedIndicator();
    }
    private void UpdateHealth()
    {
        if (useTankObjInLobbi != null)
        {
            if (useTank.healthPoint == useTank.maxHealthPoint) useTank.isMaxHealth = true;
            if (useTank.isMaxHealth) buttonUpHealth.SetActive(false);
            else if (!useTank.isMaxHealth && parameterManager.AmmoCount >= (useTank.healthPoint + 1) * 200)
            {
                buttonUpHealth.SetActive(true);
                textUpHealth.text = $"{(useTank.healthPoint + 1) * 200}";
                textHealthPoint.text = $"{useTank.maxHealth + useTank.HealthUp}";
            }
            else
            {
                buttonUpHealth.SetActive(false);
            }
        }
    }
    private void UpdateArmor()
    {
        if (useTankObjInLobbi != null)
        {
            if (useTank.armorPoint == useTank.maxArmorPoint) useTank.isMaxArmor = true;
            if (useTank.isMaxArmor) buttonUpArmor.SetActive(false);
            else if (!useTank.isMaxArmor && parameterManager.AmmoCount >= (useTank.armorPoint + 1) * 200)
            {
                buttonUpArmor.SetActive(true);
                textUpArmor.text = $"{(useTank.armorPoint + 1) * 200}";
                textArmorPoint.text = $"{useTank.armor + useTank.armorUp}";
            }
            else
            {
                buttonUpArmor.SetActive(false);
            }
        }
    }
    private void UpdateDamage()
    {
        if (useTankObjInLobbi != null)
        {
            if (useTank.damagePoint == useTank.maxDamagePoint) useTank.isMaxDamage = true;
            if (useTank.isMaxDamage) buttonUpDamage.SetActive(false);
            else if (!useTank.isMaxDamage && parameterManager.AmmoCount >= (useTank.damagePoint + 1) * 200)
            {
                buttonUpDamage.SetActive(true);
                textUpDamage.text = $"{(useTank.damagePoint + 1) * 200}";
                textDamagePoint.text = $"{useTank.damage + useTank.damageUp}";
            }
            else
            {
                buttonUpDamage.SetActive(false);
            }
        }
    }
    private void UpdateSpeed()
    {
        if (useTankObjInLobbi != null)
        {
            if (useTank.speedPoint == useTank.maxSpeedMovePoint) useTank.isMaxSpeedMove = true;
            if (useTank.isMaxSpeedMove) buttonUpSpeed.SetActive(false);
            else if (!useTank.isMaxSpeedMove && parameterManager.AmmoCount >= (useTank.speedPoint + 1) * 200)
            {
                buttonUpSpeed.SetActive(true);
                textUpSpeed.text = $"{(useTank.speedPoint + 1) * 200}";
                textSpeedPoint.text = $"{Math.Round(useTank.speedMove + useTank.speedMoveUp, 2)}";
            }
            else
            {
                buttonUpSpeed.SetActive(false);
            }
        }
    }
    private void UpdateSpeedIndicator()
    {
        float overWidth = overSpeedIndicator.rectTransform.rect.width;
        float widthInPoint = overWidth / maxSpeedInGame;
        speedIndicator.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, widthInPoint * useTank.speedMove);
    }
    private void UpdateHealthIndicator()
    {
        float overWidth = overHealthIndicator.rectTransform.rect.width;
        float widthInPoint = overWidth / maxHealthInGame;
        healthIndicator.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, widthInPoint * useTank.maxHealth);
    }
    private void UpdateArmorIndicator()
    {
        float overWidth = overArmorIndicator.rectTransform.rect.width;
        float widthInPoint = overWidth / maxArmorInGame;
        armorIndicator.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, widthInPoint * useTank.armor);
    }
    private void UpdateDamageIndicator()
    {
        float overWidth = overDamageIndicator.rectTransform.rect.width;
        float widthInPoint = overWidth / maxDamageInGame;
        damageIndicator.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, widthInPoint * useTank.damage);
    }

    public void ShowNextUpHealth()
    {
        nextUpHealth.gameObject.SetActive(true);
        float overWidth = overHealthIndicator.rectTransform.rect.width;
        float widthInPoint = overWidth / maxHealthInGame;
        nextUpHealth.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, widthInPoint * (useTank.maxHealth + useTank.HealthUp));
    }
    public void ShowNextUpDamage()
    {
        nextUpDamage.gameObject.SetActive(true);
        float overWidth = overDamageIndicator.rectTransform.rect.width;
        float widthInPoint = overWidth / maxDamageInGame;
        nextUpDamage.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, widthInPoint * (useTank.damage + useTank.damageUp));
    }
    public void ShowNextUpArmor()
    {
        nextUpArmor.gameObject.SetActive(true);
        float overWidth = overArmorIndicator.rectTransform.rect.width;
        float widthInPoint = overWidth / maxArmorInGame;
        nextUpArmor.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, widthInPoint * (useTank.armor + useTank.armorUp));
    }
    public void ShowNextUpSpeedMove()
    {
        nextUpSpeedMove.gameObject.SetActive(true);
        float overWidth = overSpeedIndicator.rectTransform.rect.width;
        float widthInPoint = overWidth / maxSpeedInGame;
        nextUpSpeedMove.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, widthInPoint * (useTank.speedMove + useTank.speedMoveUp));
    }
    public void CloseNextUpHealth()
    {
        nextUpHealth.gameObject.SetActive(false);
    }
    public void CloseNextUpArmor()
    {
        nextUpArmor.gameObject.SetActive(false);
    }
    public void CloseNextUpDamage()
    {
        nextUpDamage.gameObject.SetActive(false);
    }
    public void CloseNextUpSpeedMove()
    {
        nextUpSpeedMove.gameObject.SetActive(false);
    }

    private void SaveTanksParameters()
    {
        init.playerData.tanksParameters = "";
        Tank[] tanksOne = tanks.ToArray();
        for (int i = 0; i < tanksOne.Length; i++)
        {
            init.playerData.tanksParameters += tanksOne[i].maxHealth.ToString() + ";";
            init.playerData.tanksParameters += tanksOne[i].armor.ToString() + ";";
            init.playerData.tanksParameters += tanksOne[i].damage.ToString() + ";";
            init.playerData.tanksParameters += tanksOne[i].speedMove.ToString() + ";";
            init.playerData.tanksParameters += tanksOne[i].healthPoint.ToString() + ";";
            init.playerData.tanksParameters += tanksOne[i].armorPoint.ToString() + ";";
            init.playerData.tanksParameters += tanksOne[i].damagePoint.ToString() + ";";
            init.playerData.tanksParameters += tanksOne[i].speedPoint.ToString() + ";";
            init.playerData.tanksParameters += tanksOne[i].isMaxHealth.ToString() + ";";
            init.playerData.tanksParameters += tanksOne[i].isMaxArmor.ToString() + ";";
            init.playerData.tanksParameters += tanksOne[i].isMaxDamage.ToString() + ";";
            init.playerData.tanksParameters += tanksOne[i].isMaxSpeedMove.ToString() + ";";
            init.playerData.tanksParameters += tanksOne[i].isBuy.ToString();
            if (i + 1 != tanksOne.Length) init.playerData.tanksParameters += "\n";
        }
        init.Save();
    }
    private void LoadTanksParameters()
    {
        string[] tanksString = init.playerData.tanksParameters.Split("\n");
        for (int i = 0; i < tanksString.Length; i++)
        {
            string[] tankParameters = tanksString[i].Split(';');
            tanks[i].maxHealth = int.Parse(tankParameters[0]);
            tanks[i].armor = int.Parse(tankParameters[1]);
            tanks[i].damage = int.Parse(tankParameters[2]);
            tanks[i].speedMove = float.Parse(tankParameters[3]);
            tanks[i].healthPoint = int.Parse(tankParameters[4]);
            tanks[i].armorPoint = int.Parse(tankParameters[5]);
            tanks[i].damagePoint = int.Parse(tankParameters[6]);
            tanks[i].speedPoint = int.Parse(tankParameters[7]);
            tanks[i].isMaxHealth = Convert.ToBoolean(tankParameters[8]);
            tanks[i].isMaxArmor = Convert.ToBoolean(tankParameters[9]);
            tanks[i].isMaxDamage = Convert.ToBoolean(tankParameters[10]);
            tanks[i].isMaxSpeedMove = Convert.ToBoolean(tankParameters[11]);
            tanks[i].isBuy = Convert.ToBoolean(tankParameters[12]);
        }
    }
}
