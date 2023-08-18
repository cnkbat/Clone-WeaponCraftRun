using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class SlidingGate : MonoBehaviour
{
    [Header("Load")]
    [SerializeField] List<GameObject> bulletsInLoad;
    [SerializeField] List<Transform> bulletInLoadTransform;
    [SerializeField] int loadValue;
    public int firstLoadInitYear,secondLoadInitYear,thirdLoadInitYear;
    [SerializeField] BoxCollider firstBoxCol,secondBoxCol,thirdBoxCol;

    [Header("Bucket")]
    public Transform bucketTransform;
    public List<GameObject> bulletsInBucket;
    [Header("Gate")]
    [SerializeField] Transform bulletTransform;
    [SerializeField] List<Transform> lastBulletsSpawnTransforms;
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletGap;
    [SerializeField] int bulletCounter = 0;
    [SerializeField] float moveDur= 0.1f;
    private int currentCounter;

    [Header("Visual")]
    public List<GameObject> coloredImages;
    public List<GameObject> blackandWhiteImages;
    public List<GameObject> bulletsinFirstLoad,bulletinSecondLoad, bulletsinThirdLoad;

    [SerializeField] Material greenMat;
    [SerializeField] GameObject firstGate,SecondGate;

    private void Start() 
    {
        firstBoxCol.enabled = false;
        secondBoxCol.enabled = false;
        thirdBoxCol.enabled = false;  
    }

    public void EquliazeLists()
    {
        bulletsInBucket.Distinct().ToList();
        for (int i = bulletCounter; i < bulletsInBucket.Count; i++)
        {
            if(bulletsInBucket.Count <= GameManager.instance.numOfBulletsInLoad)
            {
                currentCounter++;

                if(currentCounter >= 3)
                {   
                    LoadGate(bulletCounter);
                
                    ++bulletCounter;
                    currentCounter = 0;
                }
            }
        } 

    }

    public void LoadGate(int i)
    {
        if(i >= lastBulletsSpawnTransforms.Count)
        {
            Debug.Log("break load gate");
            return;
        }

        GameObject spawnedBullet = Instantiate(bullet,bucketTransform);
        spawnedBullet.transform.DOMove(lastBulletsSpawnTransforms[i].position,moveDur,false);
        bulletsInLoad.Add(spawnedBullet);

        if(bulletsInLoad.Count >= loadValue)
        {
            firstGate.GetComponent<MeshRenderer>().material = greenMat;
        
            UnlockGate(firstBoxCol,blackandWhiteImages[0],coloredImages[0]);
            bulletsinFirstLoad.Add(bulletsInLoad[0]);
            bulletsinFirstLoad.Add(bulletsInLoad[1]); 
            bulletsinFirstLoad.Add(bulletsInLoad[2]); 
            bulletsinFirstLoad.Add(bulletsInLoad[3]); 
            bulletsinFirstLoad.Add(bulletsInLoad[4]); 

        }
        if(bulletsInLoad.Count >= loadValue * 2)
        {
            SecondGate.GetComponent<MeshRenderer>().material = greenMat;
            UnlockGate(secondBoxCol,blackandWhiteImages[1],coloredImages[1]);
            bulletinSecondLoad.Add(bulletsInLoad[5]);
            bulletinSecondLoad.Add(bulletsInLoad[6]); 
            bulletinSecondLoad.Add(bulletsInLoad[7]); 
            bulletinSecondLoad.Add(bulletsInLoad[8]); 
            bulletinSecondLoad.Add(bulletsInLoad[9]); 

        }
        if(bulletsInLoad.Count >= loadValue * 3)
        {
            UnlockGate(thirdBoxCol,blackandWhiteImages[2],coloredImages[2]);
            bulletsinThirdLoad.Add(bulletsInLoad[10]);
            bulletsinThirdLoad.Add(bulletsInLoad[11]); 
            bulletsinThirdLoad.Add(bulletsInLoad[12]); 
            bulletsinThirdLoad.Add(bulletsInLoad[13]); 
            bulletsinThirdLoad.Add(bulletsInLoad[14]);
        }
    }

    private void UnlockGate(BoxCollider collider,GameObject blackandWhiteImage,GameObject coloredImage)
    {
        collider.enabled = true;
        blackandWhiteImage.SetActive(false);
        coloredImage.SetActive(true);
    }

    public void PlayLoadingAnim(List<GameObject> newBulletsinLoad)
    {
        for (int i = 0; i < newBulletsinLoad.Count; i++)
        {
            float moveTransform = newBulletsinLoad[i].transform.position.y + 1f;
            newBulletsinLoad[i].transform.DOMoveY(moveTransform,0.2f);
            newBulletsinLoad[i].transform.DOScale(0,0.2f);
        }
        for (int i = 0; i < bulletsInLoad.Count; i++)
        {
            bulletsInLoad[i].GetComponent<Rigidbody>().useGravity = true;
            bulletsInLoad[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }

    public void LockAllGates()
    {
        firstBoxCol.enabled = false;
        secondBoxCol.enabled = false;
        thirdBoxCol.enabled = false;
    }
}
