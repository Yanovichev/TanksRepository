using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tank
{
    public GameObject prefab, prefabLobbi;
    public int maxHealth, armor, damage;
    public float speedMove;
    public int maxHealthPoint, maxArmorPoint, maxDamagePoint, maxSpeedMovePoint;
    public int healthPoint, armorPoint, damagePoint, speedPoint;
    public int HealthUp, armorUp, damageUp;
    public float speedMoveUp;
    public int cost;
    public bool isMaxHealth, isMaxArmor, isMaxDamage, isMaxSpeedMove, isBuy;

    
}
