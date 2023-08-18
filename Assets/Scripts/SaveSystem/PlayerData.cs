using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int fireRangeValueIndex;
    public int fireRateValueIndex;
    public int initYearValueIndex, incomeValueIndex;
    public int money;
    public int stars; 
    
    public PlayerData(Player player)
    {
        level = player.currentLevelIndex;
        fireRateValueIndex = player.fireRateValueIndex;
        initYearValueIndex = player.initYearValueIndex;
        incomeValueIndex = player.incomeValueIndex;
        money = player.money;
        stars = player.stars;
        fireRangeValueIndex = player.fireRangeValueIndex;
    }
}
