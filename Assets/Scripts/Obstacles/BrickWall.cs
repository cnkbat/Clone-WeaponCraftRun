using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BrickWall : MonoBehaviour
{
    [SerializeField] List<GameObject> bricks;
    [SerializeField] float pushValue;
    [SerializeField] float destroyDelay;

    [Header("Slider")]
    [SerializeField] Slider healthBar;
    

    private void Start() 
    {
        if(transform.parent.GetComponent<Gate>())
        {
            transform.parent.GetComponent<Gate>().boxCollider.enabled = false;
        }
        healthBar.maxValue = bricks.Count;
        healthBar.minValue = 0;
        healthBar.value = bricks.Count;
        healthBar.gameObject.SetActive(false);
    }
    public void WallShot()
    {
        healthBar.gameObject.SetActive(true);
        healthBar.value = healthBar.maxValue - bricks.Count;
        int rand = Random.Range(0,bricks.Count-1);
        int randrotate = Random.Range(-360,360);
        bricks[rand].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        bricks[rand].GetComponent<Rigidbody>().useGravity = true;
        bricks[rand].GetComponent<Rigidbody>().velocity = new Vector3(0,pushValue/2,-pushValue);
        bricks[rand].transform.DORotate(new Vector3(randrotate,randrotate,randrotate),2,RotateMode.Fast);
        DestroyBrick(bricks[rand]);
        bricks.Remove(bricks[rand]);
        if(bricks.Count <= 0)
        {
            if(transform.parent.GetComponent<Gate>())
            {   
                transform.parent.GetComponent<Gate>().boxCollider.enabled = false;
            }
            Destroy(gameObject);
        }
    }
    private void DestroyBrick(GameObject newBrick)
    {
        GameObject objectToDestroy = newBrick;
        StartCoroutine(WaitForDestroy(objectToDestroy));
    }
    private IEnumerator WaitForDestroy(GameObject brick)
    {
        yield return new WaitForSeconds(destroyDelay);
        brick.SetActive(false);
    }
}
