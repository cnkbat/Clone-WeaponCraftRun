using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance {get; private set;}

    public List<int> initYearValues;
    public List<float> incomeValues;

    public List<float> fireRateValues;
    public List<float> fireRangeValues;
    
    public List<int> costs;

    private void Awake() 
    {
        if(instance ==null)
        {
            instance = this;
        }   
    }
}
