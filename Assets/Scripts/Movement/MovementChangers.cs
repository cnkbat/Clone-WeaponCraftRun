using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementChangers : MonoBehaviour
{
    [SerializeField] Material mat;
    float value;
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        value -= Time.deltaTime;
        mat.mainTextureOffset = new Vector2(1,value);
    }
}
