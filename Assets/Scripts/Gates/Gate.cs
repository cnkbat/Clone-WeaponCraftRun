using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Gate : MonoBehaviour
{
    //Variables
    [Header("Gate Values")]
    [SerializeField] float positiveValue = 4.0f;
    [SerializeField] float negativeValue = -4.0f;
    [SerializeField] float gateValue;   
    bool fireRateGate, fireRangeGate, yearGate;
    public bool isGateActive;
    float damage;
    public BoxCollider boxCollider;
    BoxCollider[] gatesBoxcolliders;

    [Header("Materials")]

    [SerializeField] Material redPrimaryMaterial;
    [SerializeField] Material greenPrimaryMaterial;

    [SerializeReference] Material greenSecondaryMat,redSecondaryMat;

    [Header("Visual")] 
    [SerializeField] TMP_Text gateOperatorText;
    [SerializeField] TMP_Text gateValueText;
    [Header("Hit Effect")]
    [SerializeField] Vector3 originalScale;
    [SerializeField] Vector3 hitEffectScale;
    [SerializeField] float hitEffectDur;
    [SerializeField] TMP_Text damageText;
    void Start()
    {
        isGateActive = true;
        tag = "Gate";
        ChooseOperation();
        UpdateGateText();
        DamageSelectionAndTextUpdate();
        originalScale = transform.localScale;
        if(transform.parent.tag == "GateManager")
        {
            if(transform.parent.GetComponentInChildren<BoxCollider>())
            {
                gatesBoxcolliders = transform.parent.GetComponentsInChildren<BoxCollider>();
            }
        }
        
    }

    private void DamageSelectionAndTextUpdate()
    {
        if(!yearGate)
        {
            int rand = Random.Range(1,3);
            damage = rand;
            damageText.text = damage.ToString();
        }
        else if(yearGate)
        {
            int rand = Random.Range(1,2);
            damage = rand;
            damageText.text = damage.ToString();
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        //according to the type of door is doing the relevant operation
        if(other.CompareTag("Bullet") && isGateActive)
        {
            if(!other.GetComponent<Bullet>().stickmansBullet)
            {
                if(!other.GetComponent<Bullet>().hasHit)
                {
                    TakeDamage();
                    other.GetComponent<Bullet>().hasHit = true;
                }
            }
            
            Destroy(other.gameObject);
        }
        else if(other.CompareTag("Player") && isGateActive)
        {
            if(transform.parent.tag == "GateManager")
            {
                foreach (var collider in gatesBoxcolliders)
                {   
                    collider.enabled = false;
                }
            }
            if(fireRateGate)
            {
                Player.instance.IncrementCurrentFireRate(gateValue);
            }
            else if(fireRangeGate)
            {
                Player.instance.IncrementInGameFireRange(gateValue);
            }
            else if(yearGate)
            {
                Player.instance.IncrementInGameInitYear(Mathf.RoundToInt(gateValue));
            }
            gameObject.SetActive(false);
        }
    }

    private void UpdateTheColorOfGate(Material newPrimaryColor, Material newSecondaryColor)
    {
        // childlarını doğru bulup ona göre update edicez
        GetComponent<MeshRenderer>().materials[0].color = newPrimaryColor.color;
        GetComponent<MeshRenderer>().materials[1].color = newSecondaryColor.color;
    }

    private void ChooseOperation()
    {
        int chooseRand = Random.Range(0,3);
        float valueRand = Random.Range(negativeValue,positiveValue);
        float halfValueRand = RoundToClosestHalf(valueRand);
        gateValue = halfValueRand;


        // textleri de ona göre yazıcaz
        if(chooseRand == 0)
        {
            fireRangeGate = true;
            gateOperatorText.text = "Fire Range";
        }
        else if(chooseRand == 1)
        {
            fireRateGate = true;
            gateOperatorText.text = "Fire Rate";
        }
        else if(chooseRand == 2)
        {
            yearGate = true;
            gateOperatorText.text = "Init Year";
            valueRand = Random.Range(-5,4);
            gateValue = valueRand;
        }

        if(gateValue >= 0)
        {
            if(GetComponent<MeshRenderer>().materials[0].color != greenPrimaryMaterial.color)
            {
                UpdateTheColorOfGate(greenPrimaryMaterial,greenSecondaryMat);
            }
        }
        else if (gateValue < 0)
        {
            if(GetComponent<MeshRenderer>().materials[0].color != redPrimaryMaterial.color)
            {
                UpdateTheColorOfGate(redPrimaryMaterial,redSecondaryMat);
            }
        }
    }
  
    private void TakeDamage()
    {
        
        gateValue += damage;
        if(yearGate)
        {
            gateValue = Mathf.Clamp(gateValue,-100,50);
        }
        GateHitEffect();
        UpdateGateText();

        if(gateValue >= 0)
        {
            UpdateTheColorOfGate(greenPrimaryMaterial,greenSecondaryMat);
        }
        else if (gateValue < 0)
        {
            UpdateTheColorOfGate(redPrimaryMaterial,redSecondaryMat);
        }
    }  
    private void UpdateGateText()
    {
        gateValueText.text = gateValue.ToString();
    }
    public float RoundToClosestHalf(float number)
    {
        float roundedValue = Mathf.Round(number * 2) / 2;
        return roundedValue;
    }
    private void GateHitEffect()
    {
        transform.DOScale(hitEffectScale,hitEffectDur).OnComplete(GateHitEffectReset);
    }

    private void GateHitEffectReset()
    {
        transform.DOScale(originalScale,hitEffectDur);
    }
}
