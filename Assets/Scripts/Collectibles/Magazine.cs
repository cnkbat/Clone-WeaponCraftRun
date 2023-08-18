using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Magazine : MonoBehaviour
{
    [Header("Capacity")]
    [SerializeField] int magazineCapacity = 6;
    [SerializeField] List<GameObject> bulletsInClip;
    [SerializeField] List<Transform> vfxPlayTransforms;

    [Header("Rotation")]
    [SerializeField] List<Vector3> rotationValues;
    [SerializeField] float rotationDur;
    int numOfBulletsInClip = 0;

    [Header("VFX")]
    [SerializeField] ParticleSystem loadVFX;

    [Header("Moving To LeftPlatform")]
    private bool isMovingTowardLeftPlatform = false;
    [SerializeField] float moveSpeedNextGate;
    [SerializeField] GameObject playerDetector, playerCol;

    [Header("Sliding Gate")]
    [SerializeField] float slidingGateDOMoveDur;
    [SerializeField] Vector3 slidingGatePosRot = new Vector3(270,0,0);

    [Header("Travel")]
    public bool canTravel;
    public Transform travelPos;

    public void MagazineShot()
    {
        bulletsInClip[numOfBulletsInClip].SetActive(true);
        loadVFX.transform.position = vfxPlayTransforms[numOfBulletsInClip].transform.position;
        loadVFX.Play();
        transform.DORotate(rotationValues[numOfBulletsInClip],rotationDur,RotateMode.Fast);
       
        numOfBulletsInClip +=1;

        if(numOfBulletsInClip >= magazineCapacity)
        {
            MoveTowardsLeftPlatform();
        }

    }
    private void Update() 
    { 
        if(isMovingTowardLeftPlatform && tag == "MovingToLeftPlatformMagazine")
        {
            playerDetector.SetActive(false);
            playerCol.SetActive(false);
            transform.position = new Vector3(transform.position.x- moveSpeedNextGate * Time.deltaTime, 
                transform.position.y, transform.position.z);
        }
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("LeftPlatform") && tag == "MovingToLeftPlatformMagazine")
        {
            MoveTowardsNextGate(other);
        }
        if (other.CompareTag("SlidingGate") && tag == "MovingToNextGateMagazine")
        {
            SlidingGateInteraction(other);
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        if(other.CompareTag("LeftPlatform") && tag == "MovingToNextGateMagazine")
        {
            MoveTowardsNextGate(other);
        }
    }

    private void MoveTowardsNextGate(Collider other)
    {
        
        isMovingTowardLeftPlatform = false;

        tag = "MovingToNextGateMagazine";
        transform.position = new Vector3(other.transform.position.x, 
                transform.position.y, transform.position.z + moveSpeedNextGate * Time.deltaTime);

    }

    public void MoveTowardsLeftPlatform()
    {
        tag = "MovingToLeftPlatformMagazine";
        isMovingTowardLeftPlatform = true;
    }

    private void SlidingGateInteraction(Collider other)
    {
        tag = "Magazine";
        isMovingTowardLeftPlatform = false;
        Vector3 positionToMove = other.GetComponent<SlidingGate>().bucketTransform.position;
        transform.DOMove(positionToMove, slidingGateDOMoveDur);
        transform.DORotate(slidingGatePosRot,slidingGateDOMoveDur).OnComplete(UnloadMagazine);
        for (int i = 0; i < bulletsInClip.Count; i++)
        {
            if(!bulletsInClip[i].activeSelf) break;
            other.GetComponent<SlidingGate>().bulletsInBucket.Add(bulletsInClip[i]);
        }
        
        other.GetComponent<SlidingGate>().EquliazeLists();
    }

    private void UnloadMagazine()
    {
        for (int i = 0; i < bulletsInClip.Count; i++)
        {
            if(!bulletsInClip[i].activeSelf) break;
            bulletsInClip[i].transform.parent = null;
            bulletsInClip[i].GetComponent<Rigidbody>().useGravity = true;
            bulletsInClip[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }

    public void MagazineTravel()
    {
        transform.DOMove(travelPos.position,GameManager.instance.magazineTravelDur).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
}
