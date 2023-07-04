using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private ParameterManager parameterManager;
    [SerializeField] private ManagerWindows managerWindows;
    private int health, maxHealth, armor;
    public FireManager fireManager;
    public bool isPlayable = false;
    public bool godMode;
    public int Armor
    {
        get { return armor; }
        set
        {
            armor = value;
        }
    }
    public int MaxHealth
    {
        get { return maxHealth; }
        set
        {
            maxHealth = value;
            if (parameterManager != null)
            {
                parameterManager.MaxHealth = value;
            }    
        }
    }
    public int Health
    {
        get { return health; }
        set
        {
            if (!godMode)
            {
                if (value <= 0)
                {
                    health = 0;
                    parameterManager.Health = 0;
                    Death();
                }
                else if (value >= maxHealth)
                {
                    health = maxHealth;
                    parameterManager.Health = maxHealth;
                }
                else
                {
                    health = value;
                    parameterManager.Health = value;
                }
            }
        }
    }

    private void Awake()
    {
    }
    public void SetStartParameters()
    {
        parameterManager = GameObject.FindGameObjectWithTag("ParameterManager").GetComponent<ParameterManager>();
        managerWindows = GameObject.FindGameObjectWithTag("ManagerWindows").GetComponent<ManagerWindows>();
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>().player = this.gameObject;
        Health = MaxHealth;
        parameterManager.MaxHealth = MaxHealth;
    }
    public void HillRise()
    {
        Health += (int)(0.2f * maxHealth);
    }
    private void Death()
    {
        managerWindows.GameOver();
    }
    public void SetDamage(int damage)
    {
        int resultDamage = damage - armor;
        if (resultDamage <= 5) resultDamage = 5;
        Health -= resultDamage;
    }
    public void Treatment(int value)
    {
        Health += value;
    }    
}
