using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Stickman : MonoBehaviour
{    
    [Header("Components")]
    private float currentFireRate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform firingPosition;
    [SerializeField] float health;
    [SerializeField] GameObject muzzleFlashVFX;
    
    public bool canShoot;
    [Header("Money")]
    [SerializeField] GameObject money;
    [SerializeField] Transform moneyTransform;

    [Header("Anim")]
    [SerializeField] Animator animator;

    [Header("Visual")]
    [SerializeField] TMP_Text healthText;

    private void Start() 
    {
        animator = GetComponent<Animator>();
        currentFireRate = GameManager.instance.stickmanFireRate;
        UpdateHealthText();
        SetAnimationsFloat(0);
    }

    private void Update() 
    {
        if(!canShoot) return;

        if(animator.GetFloat("Speed") <= 0) 
        {
            SetAnimationsFloat(1);
            FireBullet();
        }
        currentFireRate -= Time.deltaTime;
        
        if(currentFireRate <= 0)
        {
            FireBullet();
        }
    }

    private void FireBullet()
    {
        muzzleFlashVFX.SetActive(true);
        GameObject firedBullet = Instantiate(bullet, firingPosition.position, Quaternion.identity);

        firedBullet.GetComponent<Bullet>().SetRelatedWeapon(gameObject);
        firedBullet.GetComponent<Bullet>().firedPoint = firingPosition;
        firedBullet.GetComponent<Bullet>().stickmansBullet = true;
        currentFireRate = GameManager.instance.stickmanFireRate;
        StartCoroutine(MuzzleFlashoff());
    }

    IEnumerator MuzzleFlashoff()
    {
        yield return new WaitForSeconds(0.3f); // Wait for 2 seconds
        muzzleFlashVFX.SetActive(false);
        
    }
    private void SetAnimationsFloat(float newAnimSpeed)
    {
        animator.SetFloat("Speed",newAnimSpeed);
    }

    public void TakeDamage(float damage)
    {
        print("TakeDamage");
        health -= damage;
        UpdateHealthText();
        if(health <= 0)
        {

            Instantiate(money,moneyTransform.position,Quaternion.identity);

            Destroy(gameObject);
            
        }
       
    }

    private void UpdateHealthText()
    {
        healthText.text = health.ToString();
    }

}
