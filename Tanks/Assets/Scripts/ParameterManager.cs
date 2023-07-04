using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParameterManager : MonoBehaviour
{
    public GameObject bombButton, hillButton, bombBuyButton, repairBuyButton;
    [SerializeField] LobbiManager lobbiManager;
    [SerializeField] FarmManager farmManager;
    [SerializeField] SaveManager saveManager;
    [SerializeField] private Init init;
    [SerializeField] GameObject flyBomb, flyHill;
    [SerializeField] Transform targetFly;
    [SerializeField] private int maxHealth, costBomb, costRepair;
    [SerializeField] private Image healthBackGround, healthImage;
    [SerializeField] private TMP_Text[] ammoCounts;
    [SerializeField] private TMP_Text healthText, bombCountText, bombCountTextInLobbi, bombCostText, hillCountText, repairCountTextInLobbi, repairCostText;

    private void Awake()
    {
        Health = MaxHealth;
        AmmoCount = init.playerData.countAmmo;
        HillCount = init.playerData.repairCount;
        BombCount = init.playerData.bombCount;
    }
    public int BombCount
    {
        get { return init.playerData.bombCount; }
        set
        {
            init.playerData.bombCount = value;
            UpdateBombCount();
            UpdateBombCountInLobbi();
            init.Save();
            Debug.Log("Save parameter");
        }
    }
    public int HillCount
    {
        get { return init.playerData.repairCount; }
        set
        {
            init.playerData.repairCount = value;
            UpdateRepairCount();
            UpdateRepairCountInLobbi();
            init.Save();
            Debug.Log("Save parameters");
        }
    }
    public int AmmoCount
    {
        get { return init.playerData.countAmmo; }
        set
        {
            init.playerData.countAmmo = value;
            for (int i = 0; i < ammoCounts.Length;i++)
            {
                ammoCounts[i].text = value.ToString();
            }
            lobbiManager.CheckTank();
            farmManager.UpdateMinerUp();
            UpdateBombCountInLobbi();
            UpdateRepairCountInLobbi();
            init.Save();
            Debug.Log("Save parameter");
        }
    }
    public int MaxHealth
    {
        get { return maxHealth; }
        set
        {
            maxHealth = value;
            Health = value;
        }
    }
    public float Health
    {
        get { return healthImage.rectTransform.rect.width; }
        set
        {
            float overWidth = healthBackGround.rectTransform.rect.width;
            float widthInHealth = overWidth / maxHealth;
            healthImage.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, widthInHealth * value);
            healthText.text = $"{value}/{maxHealth}";
        }
    }
    private void UpdateBombCountInLobbi()
    {
        bombCostText.text = costBomb.ToString();
        bombCountTextInLobbi.text = BombCount.ToString();
        if (AmmoCount < costBomb)
        {
            bombBuyButton.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0.5f);
            Button buttonBuy = bombBuyButton.GetComponent<Button>();
            buttonBuy.onClick.RemoveAllListeners();
        }
        else
        {
            bombBuyButton.GetComponent<Image>().color = new Color(255f, 255f, 255f, 1f);
            Button buttonBuy = bombBuyButton.GetComponent<Button>();
            buttonBuy.onClick.RemoveAllListeners();
            buttonBuy.onClick.AddListener(BuyBomb);
        }
    }
    private void UpdateRepairCountInLobbi()
    {
        repairCostText.text = costRepair.ToString();
        repairCountTextInLobbi.text = HillCount.ToString();
        if (AmmoCount < costRepair)
        {
            repairBuyButton.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0.5f);
            Button buttonBuy = repairBuyButton.GetComponent<Button>();
            buttonBuy.onClick.RemoveAllListeners();
        }
        else
        {
            repairBuyButton.GetComponent<Image>().color = new Color(255f, 255f, 255f, 1f);
            Button buttonBuy = repairBuyButton.GetComponent<Button>();
            buttonBuy.onClick.RemoveAllListeners();
            buttonBuy.onClick.AddListener(BuyRepair);
        }
    }
    private void UpdateBombCount()
    {
        bombCountText.text = BombCount.ToString(); 
        if (BombCount > 0) bombButton.SetActive(true);
        else bombButton.SetActive(false);
    }
    private void UpdateRepairCount()
    {
        hillCountText.text = HillCount.ToString();
        if (HillCount > 0) hillButton.SetActive(true);
        else hillButton.SetActive(false);
    }
    private void BuyRepair()
    {
        AmmoCount -= costRepair;
        HillCount++;
    }
    private void BuyBomb()
    {
        AmmoCount -= costBomb;
        BombCount++;
    }
    public void CreateFlyBomb()
    {
        BombCount--;
        GameObject fly = Instantiate(flyBomb, targetFly);
        fly.GetComponent<FlyBomb>().Notify += UpdateBombCount;
        fly.transform.SetParent(null);
        bombButton.SetActive(false);
    }
    public void CreateFlyRepair()
    {
        HillCount--;
        GameObject fly = Instantiate(flyHill, targetFly);
        fly.GetComponent<FlyRepair>().Notify += UpdateRepairCount;
        fly.transform.SetParent(null);
        hillButton.SetActive(false);
    }

}
