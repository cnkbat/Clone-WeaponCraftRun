using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Cinemachine;

public class GameManager : MonoBehaviour
{   
    public static GameManager instance;

    //ending level related variables
    [Header("Phase variables")]
    public bool gameHasEnded = false;
    public bool gameHasStarted = false;
    //----------------

    [Header("Player Attributes")]
    public float playerDamage;

    [Header("Stickman")]
    public float stickmanFireRate;
    public float stickmanFireRange;

    [Header("Sliding Gate")]
    public int numOfBulletsInLoad;

    [Header("Visual")]
    public GameObject mainCam,startingCam;
    Vector3 camStartingPos;

    [Header("Ending")]
    public GameObject endSniper;
    public Vector3 rotationSpeed = new Vector3(100, 0, 0); // Adjust the rotation speed as needed

    [Header("Magazine")]
    public float magazineTravelDur;
    

    ////
    ////   ***********<SUMMARY>*************
    //// Game manager script is alive every scene.
    /// this script manages all the scores and hat related stuff in the game
    /// and all the huds that player can and cant see.
    /// as you can see at the bottom this script also finishes the game.


    // assigning variables
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        } 
       
    }
    private void Start()
    {
        UpdatePlayerDamage();
        camStartingPos = mainCam.transform.localPosition;
    }

    public void UpdatePlayerDamage()
    {
        playerDamage = Player.instance.playerDamage;
    }

    private void Update() 
    {
        endSniper.transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    public void EndLevel()
    {
        gameHasEnded = true;
        Player.instance.PlayerDeath();
        UIManager.instance.FinishHud();
    }
    
    public void CameraStateChange()
    {
        if(startingCam.activeSelf)
        {

            mainCam.GetComponent<CinemachineBrain>().m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
            startingCam.SetActive(false);
        }
        else
        {
            mainCam.GetComponent<CinemachineBrain>().m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
            startingCam.SetActive(true);
        }   
        
    }

   
    // buttona basıldığında gerçekleşecek
    public void LoadNextScene()
    {
        Player.instance.SavePlayerData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
