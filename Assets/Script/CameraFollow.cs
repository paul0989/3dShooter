using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;
    //target=角色,transform=攝影機    
    public float smoothing = 5;
    //追隨的緩衝    
    private Vector3 offset;
    //跟攝影機之間的差距

    // Use this for initialization
    void Start()
    {
        offset = transform.position - target.position;
        //差距=攝影機的位置-目標的位置
    }

    private void FixedUpdate()
    {        
        Vector3 targetCameraPostion = target.position + offset;
        //camera目前要計算的位置=>目標的位置+差距        
        transform.position = Vector3.Lerp(transform.position,targetCameraPostion,Time.deltaTime* smoothing);
        //位置=現在的位置,想要變到的位置,插值(移動時間)
    }
}
