using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazinePlayerDetector : MonoBehaviour
{
    [SerializeField] GameObject relatedMagazine;

    private void Start() 
    {
        relatedMagazine = transform.parent.gameObject;
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))    
        {
            if(relatedMagazine.GetComponent<Magazine>().canTravel)
            {
                relatedMagazine.GetComponent<Magazine>().MagazineTravel();
            }
        }
    }
}
