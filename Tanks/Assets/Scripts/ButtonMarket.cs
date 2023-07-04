using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonMarket : MonoBehaviour
{
    [SerializeField] private MarketManager marketManager;
    [SerializeField] private int ammo, bomb, repair, cost;
    [SerializeField] private TMP_Text ammoText, bombText, repairText, costText;
    private void Awake()
    {
        UpdateText();
    }
    public void Buy()
    {
        marketManager.Buy(ammo, bomb, repair, cost);
    }
    private void UpdateText()
    {
        ammoText.text = $"{ammo}";
        bombText.text = $"+{bomb}";
        repairText.text = $"+{repair}";
        costText.text = $"{cost} Yan";
    }
}
