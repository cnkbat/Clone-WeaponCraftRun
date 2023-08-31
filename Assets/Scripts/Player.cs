using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;
using System.IO;

public class Player : MonoBehaviour
{
    public static Player instance {get; private set;}
    //Component
    CapsuleCollider capsuleCollider;
    Rigidbody rBody;
    BoxCollider boxCollider;

    [Header("Movement")]
    [SerializeField] private float forwardMoveSpeed;
    [SerializeField] private float negativeLimitValue, positiveLimitValue,maxSwerveAmountPerFrame;
    private float _lastXPos;

    [Header("Movement Changers")]
    public bool knockbacked = false;
    [SerializeField] float knockbackValue = 10f ;
    [SerializeField] float knockbackDur = 0.4f;
    [SerializeField] float slowMovSpeed;
    [SerializeField] float fastMovSpeed;
    [SerializeField] float originalMoveSpeed;


    [Header("Saved Attributes")]
    public int initYear;
    public float income = 1;
    public float fireRate, fireRange;

    [Tooltip("Current Attributes")]
    private int inGameInitYear;
    private float inGameFireRate,inGameFireRange;
    
    [Header("Weapon")]
    [SerializeField] GameObject currentWeapon;

    [Header("Weapon Selecting")] 
    [SerializeField] List<GameObject> weapons;
    public List<int> weaponChoosingInitYearsLimit;

    [Header("Upgrade Index")]
        [Tooltip("Save & Load Value")]
    // we will save and load thorugh this header and set the values after

    public int fireRateValueIndex;
    public int fireRangeValueIndex, initYearValueIndex, incomeValueIndex;
    public int money;
    public int stars; 
    public int currentLevelIndex;
    public float playerDamage;

    [HideInInspector]
    public int weaponIndex;

    [Header("Death")]
    [SerializeField] Vector3 deathEndValue;
    [SerializeField] float deathDur;

    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        } 
    }

    void Start() 
    {
        rBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        currentWeapon = GameObject.FindGameObjectWithTag("Weapon");
        boxCollider = GetComponent<BoxCollider>();
        tag = "Player";
        LoadPlayerData();
        SetUpgradedValues();
        UpdatePlayersDamage();

        originalMoveSpeed = forwardMoveSpeed;
    }

    void Update() 
    {   
        if(!GameManager.instance.gameHasStarted) return;
        if(GameManager.instance.gameHasEnded) return;

        if(!GameManager.instance.gameHasEnded)
        {
            if(!knockbacked)
            {
                MoveCharacter();
            }
        }
    }

    private void MoveCharacter()
    {
        Vector3 moveDelta = Vector3.forward;
        if (Input.GetMouseButtonDown(0))
        {
            _lastXPos = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0))
        {
            float moveXDelta = Mathf.Clamp(Input.mousePosition.x - _lastXPos, -maxSwerveAmountPerFrame,
                maxSwerveAmountPerFrame);
            moveDelta += new Vector3(moveXDelta, 0, 0);
            _lastXPos = Input.mousePosition.x;
        }

        moveDelta *= Time.deltaTime * forwardMoveSpeed;

        Vector3 currentPos = transform.position;
        Vector3 newPos = new Vector3(
            Mathf.Clamp(currentPos.x + moveDelta.x, -negativeLimitValue, positiveLimitValue),
            currentPos.y,
            currentPos.z + moveDelta.z);
        transform.position = newPos;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("MovementSlower"))
        {
            SetMovementSpeed(slowMovSpeed);
        }
        else if(other.CompareTag("MovementFaster"))
        {
            SetMovementSpeed(fastMovSpeed);
        }
        else if(other.CompareTag("Obstacle"))   
        {
            Player.instance.KnockbackPlayer();
            other.tag = "Untagged";
        }
        else if(other.CompareTag("Stickman"))   
        {
            Player.instance.KnockbackPlayer();
            other.tag = "TouchedStickman";
        }
        else if(other.CompareTag("BrickWall"))
        {
            Player.instance.KnockbackPlayer();
            other.tag = "TouchedBrickWall";
        }
        else if(other.CompareTag("StickmansPlayerDetector"))
        {
            other.transform.parent.GetComponent<Stickman>().canShoot = true;
        }
        else if(other.CompareTag("EndingObstacle"))
        {
            GameManager.instance.EndLevel();
        }
        else if(other.CompareTag("MagazinesPlayerCol"))
        {
            other.transform.parent.GetComponent<Magazine>().MoveTowardsLeftPlatform();
        }
        else if(other.CompareTag("Money"))
        {
            IncrementMoney(other.GetComponent<Money>().value);
            Destroy(other.gameObject);
        }
        else if(other.CompareTag("FirstSlidingGateCol"))
        {
            IncrementInGameInitYear(other.transform.parent.parent.GetComponent<SlidingGate>().firstLoadInitYear);
            
            other.transform.parent.parent.GetComponent<SlidingGate>().
                PlayLoadingAnim(other.transform.parent.parent.GetComponent<SlidingGate>().bulletsinFirstLoad);

            other.transform.parent.parent.GetComponent<SlidingGate>().LockAllGates();
        }
        else if(other.CompareTag("SecondSlidingGateCol"))
        {
            IncrementInGameInitYear(other.transform.parent.parent.GetComponent<SlidingGate>().secondLoadInitYear);

            other.transform.parent.parent.GetComponent<SlidingGate>().
                PlayLoadingAnim(other.transform.parent.parent.GetComponent<SlidingGate>().bulletinSecondLoad);

            other.transform.parent.parent.GetComponent<SlidingGate>().LockAllGates();
        }
        else if(other.CompareTag("ThirdSlidingGateCol"))
        {
            IncrementInGameInitYear(other.transform.parent.parent.GetComponent<SlidingGate>().thirdLoadInitYear);

            other.transform.parent.parent.GetComponent<SlidingGate>().
                PlayLoadingAnim(other.transform.parent.parent.GetComponent<SlidingGate>().bulletsinThirdLoad);

            other.transform.parent.parent.GetComponent<SlidingGate>().LockAllGates();
        }
        else if(other.CompareTag("FinishLine"))
        {
            GameManager.instance.CameraStateChange();
        }
        else if(other.CompareTag("Chain"))
        {
            KnockbackPlayer();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("MovementSlower"))
        {
            SetMovementSpeed(originalMoveSpeed);
        }
        else if(other.CompareTag("MovementFaster"))
        {
            SetMovementSpeed(originalMoveSpeed);
        }
    }

    private void WeaponSelector()
    {
        
        if(inGameInitYear <= weaponChoosingInitYearsLimit[0] && currentWeapon != weapons[0])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }
            currentWeapon = weapons[0];
            weaponIndex = 0;
            currentWeapon.SetActive(true);
        }
        if(inGameInitYear > weaponChoosingInitYearsLimit[0] && initYear <= weaponChoosingInitYearsLimit[1] && currentWeapon != weapons[1]) 
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            currentWeapon = weapons[1];
            weaponIndex = 1;
            currentWeapon.SetActive(true);
        }
        if(inGameInitYear > weaponChoosingInitYearsLimit[1] && initYear <= weaponChoosingInitYearsLimit[2] && currentWeapon != weapons[2])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            currentWeapon = weapons[2];
            weaponIndex = 2;
            currentWeapon.SetActive(true);
        }
        if(inGameInitYear > weaponChoosingInitYearsLimit[2] && initYear <= weaponChoosingInitYearsLimit[3] && currentWeapon != weapons[3])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            weaponIndex = 3;
            currentWeapon = weapons[3];
            currentWeapon.SetActive(true);
        }

        if(inGameInitYear > weaponChoosingInitYearsLimit[3] && initYear <= weaponChoosingInitYearsLimit[4] && currentWeapon != weapons[4])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            weaponIndex = 4;
            currentWeapon = weapons[4];
            currentWeapon.SetActive(true);
        }

        if(inGameInitYear > weaponChoosingInitYearsLimit[4] && initYear <= weaponChoosingInitYearsLimit[5] && currentWeapon != weapons[5])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            weaponIndex = 5;
            currentWeapon = weapons[5];
            currentWeapon.SetActive(true);
        }
        if(inGameInitYear > weaponChoosingInitYearsLimit[5] && initYear <= weaponChoosingInitYearsLimit[6] && currentWeapon != weapons[6])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            currentWeapon = weapons[6];
            weaponIndex = 6;
            currentWeapon.SetActive(true);
        }
        if(inGameInitYear > weaponChoosingInitYearsLimit[6] && initYear <= weaponChoosingInitYearsLimit[7] && currentWeapon != weapons[7])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            weaponIndex = 7;
            currentWeapon = weapons[7];
            currentWeapon.SetActive(true);
        }
        currentWeapon.transform.parent = transform;
        UpdatePlayersDamage();
        GameManager.instance.UpdatePlayerDamage();
    }
    // player knockBack
    public void KnockbackPlayer()
    {
        knockbacked = true;
        IncrementInGameInitYear(-1);

        transform.DOMove
            (new Vector3(transform.position.x,transform.position.y, transform.position.z - knockbackValue),knockbackDur).
                OnComplete(ResetKnockback);
        UIManager.instance.UpdateInitYearText();
        
    }
    void ResetKnockback()
    {
        knockbacked = false;
    }    
    public void PlayerDeath()
    {
        DOTween.Clear(currentWeapon.gameObject);
        currentWeapon.transform.DORotate(deathEndValue, deathDur,RotateMode.Fast);
        GetComponent<BoxCollider>().isTrigger = false;
        GetComponent<Rigidbody>().useGravity = true;
    }
    public void UpdatePlayersDamage()
    {
        playerDamage = playerDamage + currentWeapon.GetComponent<Weapon>().damage;
    }

    // SAVE LOAD

    public void SavePlayerData()
    {
        SaveSystem.SavePlayerData(this);
    }
    
    public void LoadPlayerData()
    {
        SaveSystem.LoadPlayerData();
        PlayerData data = SaveSystem.LoadPlayerData();

        if(data != null)
        {
            currentLevelIndex = data.level;
            fireRateValueIndex = data.fireRateValueIndex;
            initYearValueIndex = data.initYearValueIndex;
            incomeValueIndex = data.incomeValueIndex;
            money = data.money;
            stars = data.stars;
            fireRangeValueIndex = data.fireRangeValueIndex;
        }
    }

    // Getters And Setters
    private void SetMovementSpeed(float newMoveSpeed)
    {
        forwardMoveSpeed = newMoveSpeed;
    }
    public void SetUpgradedValues()
    {
        initYear = UpgradeManager.instance.initYearValues[initYearValueIndex];
        fireRate = UpgradeManager.instance.fireRateValues[fireRateValueIndex];
        fireRange = UpgradeManager.instance.fireRangeValues[fireRangeValueIndex];
        income = UpgradeManager.instance.incomeValues[incomeValueIndex];
        SetStartingValues();
    }
    private void SetStartingValues()
    {
        inGameFireRange = fireRange;
        inGameFireRate = fireRate;
        inGameInitYear = initYear;
        WeaponSelector();
    }
    public int GetInGameInitYear()
    {
        return inGameInitYear;
    }
    public float GetInGameFireRange()
    {
        return inGameFireRange;
    }
    public float GetInGateFireRate()
    {
        return inGameFireRate;
    }

    //SETTERS
    public void IncrementInGameFireRange(float value)
    {
        inGameFireRange += value;
    }
    public void IncrementCurrentFireRate(float value)
    {
        float effectiveValue = value / 100;
        Debug.Log(effectiveValue);
        inGameFireRate -= effectiveValue;
    }
    public void IncrementInGameInitYear(int value)
    {
        if(value == -1) 
        {
            Debug.Log("display");
            UIManager.instance.DisplayInitYearReduce();
        }
        inGameInitYear += value;
        UIManager.instance.UpdateInitYearText();
        UIManager.instance.UpdateWeaponBar();

        WeaponSelector();
    }
    public void IncrementMoney(int value)
    {
        money += Mathf.RoundToInt(value * income);
        UIManager.instance.UpdateMoneyText();
    }
}
