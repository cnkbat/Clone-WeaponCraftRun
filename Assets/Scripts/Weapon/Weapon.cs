using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Weapon : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] GameObject bullet;

    [Header("Attributes")]
    [SerializeField] Transform tipOfWeapon;

    [Header("Firing")]
    [SerializeField] float firedRotationDelay = 0.2f, resetRotationDelay = 0.8f;
    
    [SerializeField] Vector3 fireEndRotationValue = new Vector3(315,0,0);
    [SerializeField] Vector3 originalRotationValue;

    [Header("Attributes")]
    [SerializeField] float fireRange;
    [SerializeField] float fireRate;
    private float currentFireRate;
    public float damage;

    [Header("VFX")]
    [SerializeField] GameObject muzzleFlashVFX;

    private void Start() 
    {   
        tag = "Weapon";
    }

    private void ResetPos()
    {
        transform.DORotate(originalRotationValue,resetRotationDelay,RotateMode.Fast);
    }
    private void Update() 
    {
        if(!GameManager.instance.gameHasStarted) return;
        if(GameManager.instance.gameHasEnded) return;
        
        if(Player.instance.knockbacked)
        {
            UpdateFireRate();
            return;
        }
        
        currentFireRate -= Time.deltaTime;
        
            if(currentFireRate <= 0)
            {
                FireBullet();
                UpdateFireRate();
            }
    }
    public void FireBullet()
    {
        muzzleFlashVFX.SetActive(true);

        transform.DORotate(fireEndRotationValue,firedRotationDelay,RotateMode.Fast).
            OnComplete(ResetPos);
            

        GameObject firedBullet = Instantiate(bullet, tipOfWeapon.position ,Quaternion.identity);
        
        firedBullet.GetComponent<Bullet>().firedPoint = tipOfWeapon;
        firedBullet.GetComponent<Bullet>().SetRelatedWeapon(gameObject);
        StartCoroutine(MuzzleFlashoff());
    }
    IEnumerator MuzzleFlashoff()
    {
        yield return new WaitForSeconds(0.3f); // Wait for 2 seconds
        muzzleFlashVFX.SetActive(false);
        
    }
    public float GetWeaponsFireRange()
    {
        return Player.instance.GetInGameFireRange() + fireRange;
    }
    private void UpdateFireRate()
    {
        currentFireRate = Player.instance.GetInGateFireRate() + fireRate;
    }

}
