using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketStatus : MonoBehaviour
{
    // Start is called before the first frame update
    
    //设置水桶质量
    public float emptyBucketMass = 1.0f;
    public float gasMass = 4.0f;
    public float bucketMass = 0.0f;
    
    //设置水桶序号
    public int bucketNumber = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bucketMass = emptyBucketMass + gasMass;
        
    }
}
