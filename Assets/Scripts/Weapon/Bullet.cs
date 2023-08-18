using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    public Transform firedPoint;
    Weapon relatedWeaponComponent;
    BoxCollider boxCollider;

    [SerializeField] float moveSpeed;
    private Vector3 firedPointCurrent;
    private float fireDist;
    [SerializeField] GameObject relatedWeapon; 

    [Tooltip("Stickman Firing")]
    public bool stickmansBullet = false;

    [SerializeField] Vector3 rotationValue;

    [SerializeField] GameObject hitEffect;
 
    private void Start() 
    {
        firedPointCurrent = firedPoint.position;
        if(!stickmansBullet)
        {
            fireDist =  relatedWeapon.GetComponent<Weapon>().GetWeaponsFireRange();
        }
        else if(stickmansBullet)
        {
            fireDist = GameManager.instance.stickmanFireRange;
        }

        transform.DORotate(rotationValue,0f);
    }

    void Update()
    {
        if(!stickmansBullet)
        {
            if(!(Vector3.Distance(firedPointCurrent,transform.position) > fireDist))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + moveSpeed * Time.deltaTime);
            }   
            else
            {
                Destroy(gameObject); //setactive false da olabilir
            }
        }
        else if(stickmansBullet)
        {
            if(!(Vector3.Distance(firedPointCurrent,transform.position) > fireDist ))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 
                    transform.position.z - moveSpeed * Time.deltaTime);
            }
            else
            {
                Destroy(gameObject); //setactive false da olabilir
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Untagged"))
        {
            PlayHitFX();
            Destroy(gameObject);
        }
        if (other.CompareTag("EndingObstacle"))
        {
            if (!stickmansBullet)
            {
                PlayHitFX();
                other.GetComponent<EndingObstacle>().TakeDamage(GameManager.instance.playerDamage);
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("SlidingGate"))
        {
            PlayHitFX();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Magazine"))
        {
            if (!stickmansBullet)
            {
                PlayHitFX();
                other.GetComponent<Magazine>().MagazineShot();
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            if (stickmansBullet)
            {
                // UI EKSİLME
                Player.instance.IncrementInGameInitYear(-1);
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("Stickman"))
        {
            if (!stickmansBullet)
            {
                PlayHitFX();
                other.GetComponent<Stickman>().TakeDamage(GameManager.instance.playerDamage);
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("TouchedStickman"))
        {
            if (!stickmansBullet)
            {
                PlayHitFX();
                other.GetComponent<Stickman>().TakeDamage(GameManager.instance.playerDamage);
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("BrickWall"))
        {
            if (!stickmansBullet)
            {
                PlayHitFX();
                other.GetComponent<BrickWall>().WallShot();
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("TouchedBrickWall"))
        {
            if (!stickmansBullet)
            {
                PlayHitFX();
                other.GetComponent<BrickWall>().WallShot();
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("Chain"))
        {
            if (!stickmansBullet)
            {
                PlayHitFX();
                other.GetComponent<Chain>().TakeDamage(1);
                Destroy(gameObject);
            }
        }
    }

    private void PlayHitFX()
    {
        if (hitEffect != null)
        {
            GameObject hitfx = Instantiate(hitEffect, transform.position, Quaternion.identity);
            hitfx.GetComponent<ParticleSystem>().Play();
        }
    }

    public void SetRelatedWeapon(GameObject newWeapon)
    {
        relatedWeapon = newWeapon;
    }
}
