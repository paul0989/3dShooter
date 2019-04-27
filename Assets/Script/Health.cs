using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public float RotationSpeed = 90;//旋轉速度
    public int Treatment = 10;//治療量
    //private GameObject HealthCap;
    private bool PlayerTouch;//player是否碰到
    private PlayerHealth playerHealth;//碰到時呼叫
    //補血的CD時間
    private float timer;
    private float timeBetweenHealth = 0.5f;

    private bool playerIsDeath = false;
    private bool EatHealth;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }
    private void PlayerHealthAction()
    {
        playerIsDeath = false;
    }


    private void Start()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    private void OnEnable()
    {
        PlayerHealth.PlayerHealthEvent += PlayerHealthAction;
    }
    private void OnDisable()
    {
        PlayerHealth.PlayerHealthEvent -= PlayerHealthAction;
    }

    private void OnTriggerEnter(Collider other)//Player在補血範圍內
    {
        if (other.tag == playerHealth.tag)
        {
            PlayerTouch = true;
        }
    }

    private void OnTriggerExit(Collider other)//離開這個Trigger
    {
        if (other.tag == playerHealth.tag)
        {
            PlayerTouch = false;
        }
    }

    private void HealPlayer()
    {
        timer = 0;//治療後歸0重新計算下次治療膠囊時間
        playerHealth.TakeHeal(Treatment);
    }


    // Update is called once per frame
    void Update()
    {
        //補血物件旋轉
        transform.Rotate(Vector3.forward * Time.deltaTime * RotationSpeed);//向量
        timer += Time.deltaTime;//每個frame做deltaTime的增加
        //player在範圍內And player沒死(playerIsDeath==false)
        if (PlayerTouch && playerIsDeath == false)
        {
            if (timer >= timeBetweenHealth)
            {
                HealthManager._Instance.HealthObjectCostEnergy(this.gameObject);
                PlayerTouch = false;
                HealPlayer();
            }

        }

    }
    public void ChangeSpeed(float NewSpeed)
    {
        this.RotationSpeed = NewSpeed;
    }

}
