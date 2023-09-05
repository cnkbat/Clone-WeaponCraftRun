using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance {get; private set;}

    [Header("Income")]
     public List<float> incomeValues;
    [SerializeField] float incomeStartValue, incomeIncreasingValue;
    [Header("InitYear")]
    public List<int> initYearValues;
    [SerializeField] int initYearStartValue, initYearIncreasingValue;
    
    [Header("Fire Rate")]
    public List<float> fireRateValues;
    [SerializeField] float fireRateStartValue, fireRateIncreasingValue, secondFireRateIncreasingValue;
    [SerializeField] int fireRateChangeLevelIndex;

    [Header("Fire Range")]
    public List<float> fireRangeValues;
    [SerializeField] float fireRangeStartValue, fireRangeIncreasingValue;

    [Header("Cost")]
    public List<int> costs;
    [SerializeField] int costStartingValue, costIncreasingValue;

    private void Awake() 
    {
        if(instance ==null)
        {
            instance = this;
        }   
        initYearValues.Clear();
        costs.Clear();
        fireRangeValues.Clear();
        fireRateValues.Clear();
        incomeValues.Clear();

        SetIncomeValues();
        SetFireRangeValues();
        SetFireRateValues();
        SetInitYearValues();
        SetCostValues();
    }
    public void SetInitYearValues()
    {
        int firstValue = initYearStartValue;
        initYearValues.Add(firstValue);

        for(int i = 1; i < 1000 ; i++)
        {
            int valueNext = initYearValues[i - 1] + initYearIncreasingValue;
            initYearValues.Add(valueNext);
        }
    }
    public void SetFireRangeValues()
    {
        float firstValue = fireRangeStartValue;
        fireRangeValues.Add(firstValue);

        for (int i = 1; i < 1000; i++)
        {
            float nextValue = fireRangeValues[i - 1] + fireRangeIncreasingValue;
            fireRangeValues.Add(nextValue);
        }
    } 
    public void SetIncomeValues()
    {
        float firstValue = incomeStartValue;
        incomeValues.Add(firstValue);

        for (int i = 1; i < 1000; i++)
        {
            float nextValue = incomeValues[i - 1] + incomeIncreasingValue;
            nextValue = Mathf.Round(nextValue * 100f) / 100f;
            incomeValues.Add(nextValue);
        }
    }
    public void SetFireRateValues()
    {
        float firstValue = fireRateStartValue;
        fireRateValues.Add(firstValue);

        for (int i = 1; i < fireRateChangeLevelIndex; i++)
        {
            float nextValue = fireRateValues[i - 1] - fireRateIncreasingValue;
            nextValue = Mathf.Round(nextValue * 100f) / 100f;
            fireRateValues.Add(nextValue);
        }
        for (int i = fireRateChangeLevelIndex; i < 1000; i++)
        {
            float nextValue = fireRateValues[i -1] - secondFireRateIncreasingValue;
            nextValue = Mathf.Round(nextValue * 100f) / 100f;
            nextValue = Mathf.Clamp(nextValue,0.04f,5f);
            fireRateValues.Add(nextValue);
        }
    } 
    public void SetCostValues()
    {
        int firstValue = costStartingValue;
        costs.Add(firstValue);
        for (int i = 1; i < 1000; i++)
        {
            int nextValue = costs[i - 1] + costIncreasingValue;
            costs.Add(nextValue);
        }
    }
}
