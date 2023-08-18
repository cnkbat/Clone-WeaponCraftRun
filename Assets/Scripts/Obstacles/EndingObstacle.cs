using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class EndingObstacle : MonoBehaviour
{
    [SerializeField] float health = 10;
    [SerializeField] GameObject money;
    [SerializeField] Transform moneySpawnTransform;
    [SerializeField] int moneysValue; 
    [SerializeField] TMP_Text healthText;
    [SerializeField] Vector3 hitEffectScale;
    [SerializeField] float hitEffectDur;
    private Vector3 originalScale;
    private void Start() 
    {
        originalScale = healthText.transform.localScale;
        UpdateHealthText();        
    }
    public void TakeDamage(float dmg)
    {
        health -= dmg;
        UpdateHealthText();
        ObstacleHitEffect();
        if(health <= 0)
        {
            GameObject spawnedMoney = Instantiate(money,moneySpawnTransform.position,Quaternion.identity);
            spawnedMoney.GetComponent<Money>().value = moneysValue;
            Destroy(gameObject);
        }
    }

    public void UpdateHealthText()
    {
        healthText.text = Mathf.RoundToInt(health).ToString();
    }

    private void ObstacleHitEffect()
    {
        healthText.transform.DOScale(hitEffectScale,hitEffectDur).OnComplete(ObstacleHitEffectReset);
    }

    private void ObstacleHitEffectReset()
    {
        healthText.transform.DOScale(originalScale,hitEffectDur);
    }

}
