using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Chain : MonoBehaviour
{
    [SerializeField] float maxHealth = 10;
    [SerializeField] Slider slider;
    float currentHealth;
    [SerializeField] List<GameObject> chains;
    [SerializeField] GameObject fill;
    float damagedValue;
    private void Start() 
    {
        if(transform.parent.GetComponent<Gate>())
        {
            transform.parent.GetComponent<Gate>().boxCollider.enabled = false;
        }

        fill.SetActive(false);

        currentHealth = maxHealth;
        slider.maxValue = maxHealth;
        slider.minValue = 0;
        slider.value = damagedValue;
    }
    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        damagedValue += dmg;
        Debug.Log(currentHealth + " health");
        Debug.Log("damagedValue" + damagedValue);
        fill.SetActive(true);
        slider.value = damagedValue;

        if(currentHealth <= 0)
        {
            transform.parent.GetComponent<Gate>().boxCollider.enabled = true;
            Destroy(gameObject);
        }
    }
}
