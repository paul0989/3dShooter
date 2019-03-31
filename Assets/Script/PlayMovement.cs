using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMovement : MonoBehaviour {
    public float speed = 6f;
    private Vector3 movement;
    private Animator anim;
    private Rigidbody playerRigibody;
    private float camRayLenght = 100f;//旋轉用,射線對準地板,長度100
    private int floorMask;//射線對著地板

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerRigibody = GetComponent<Rigidbody>();
        floorMask = LayerMask.GetMask("floor");
    }

    void Turning()
    {
        //main=>main Camera設定一條射線(從攝影機的視線)往滑鼠的位置
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //射線打到floor
        RaycastHit floorHit;
        if (Physics.Raycast(camRay,out floorHit, camRayLenght, floorMask))
        {
            //角色視線旋轉的方向計算     要看的方向    -  現在看的位置
            Vector3 playerToMouse = floorHit.point - transform.position;
            //y歸0
            playerToMouse.y = 0;
            //看向這個方向
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigibody.MoveRotation(newRotation);
        }
    }

    void Move(float h, float v)//水平.垂直
    {
        movement.Set(h,0f,v);//X,Y,Z 沒有跳躍(Y)
        movement = movement.normalized * speed * Time.deltaTime;//normalized只取方向,speed=第六行速度,Time.deltaTime=上一個frame跟下一個frame的間隔
        playerRigibody.MovePosition(transform.position+movement);
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0 || v != 0;//h不等於0或著v不等於0,其中一個不是0等於有在移動
        anim.SetBool("isWalking",walking);
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");//GetAxisRaw直接就是1,GetAxis則是慢慢漸進
        float v = Input.GetAxisRaw("Vertical");
        //Debug.Log(h+":"+v);
        //呼叫
        Move(h, v);
        Turning();
        Animating(h, v);
    }
}
