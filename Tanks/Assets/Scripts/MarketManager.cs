using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MarketManager : MonoBehaviour
{
    [SerializeField] private ParameterManager parameterManager;

    public void Buy(int ammo, int bomb, int repair, int cost)
    {
            parameterManager.AmmoCount += ammo;
            parameterManager.BombCount += bomb;
            parameterManager.HillCount += repair;
            Init.Instance.Save();
    }
}
