using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    //target=角色,transform=攝影機
    public Transform target;
    //追隨的緩衝
    public float smoothing = 5;
    //跟攝影機之間的差距
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        //差距=攝影機的位置-目標的位置
        offset = transform.position - target.position;

	}

    private void FixedUpdate()
    {
        //camera目前要計算的位置=>目標的位置+差距
        Vector3 targetCameraPostion = target.position + offset;
        //位置=現在的位置,想要變到的位置,插值(移動時間)
        transform.position = Vector3.Lerp(transform.position,targetCameraPostion,Time.deltaTime* smoothing);
    }
}
